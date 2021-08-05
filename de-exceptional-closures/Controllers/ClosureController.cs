﻿using AutoMapper;
using de_exceptional_closures.Notify;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Closure;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Data;
using de_exceptional_closures_infraStructure.Features.ClosureReason.Commands;
using de_exceptional_closures_infraStructure.Features.ClosureReason.Queries;
using de_exceptional_closures_infraStructure.Features.ReasonType.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class ClosureController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly INotifyService _notifyService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private static readonly NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public ClosureController(IMediator mediator, IMapper mapper, INotifyService notifyService, SignInManager<ApplicationUser> signInManager,
             UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _notifyService = notifyService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult DateFrom(bool isSingleDay)
        {
            DateFromViewModel model = new DateFromViewModel();

            model.InstitutionName = GetInstitutionName();

            model.TitleTagName = "From what date did you close, or plan to close, " + model.InstitutionName + " ?";
            model.IsSingleDay = isSingleDay;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DateFrom(DateFromViewModel model)
        {
            model.TitleTagName = "From what date did you close, or plan to close, " + model.InstitutionName + " ?";

            if (!ModelState.IsValid)
            {
                //         model.ErrorClass = "govuk-form-group--error";

                return View(model);
            }

            LogAudit("opened DateFrom POST view");

            DateTime datefrom;

            // Check for valid dates
            if (DateTime.TryParse(CreateDate(model.DateFromYear.ToString(), model.DateFromMonth.ToString(), model.DateFromDay.ToString()), out datefrom))
            {
                model.DateFrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day);
            }
            else
            {
                LogAudit("Encountered an error: Please enter in a valid date. DateFrom POST view.");
                ModelState.AddModelError("DateFrom", "Please enter in a valid date");
                return View(model);
            }


            if (model.IsSingleDay)
            {
                return RedirectToAction("RequestClosure", "Closure", new { dateFrom = model.DateFrom });
            }

            return RedirectToAction("DateTo", "Closure", new { dateFrom = model.DateFrom });
        }

        [HttpGet]
        public IActionResult DateTo(DateTime dateFrom)
        {
            DateToViewModel model = new DateToViewModel();
            model.DateFrom = dateFrom;
            model.InstitutionName = GetInstitutionName();

            model.TitleTagName = "From what date did you close, or plan to close, " + model.InstitutionName + " ?";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DateTo(DateToViewModel model)
        {
            model.TitleTagName = "From what date did you close, or plan to close " + model.InstitutionName + " ?";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DateTime dateTo;

            // Check for valid dates
            if (DateTime.TryParse(CreateDate(model.DateToYear.ToString(), model.DateToMonth.ToString(), model.DateToDay.ToString()), out dateTo))
            {
                model.DateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day);
            }
            else
            {
                LogAudit("Encountered an error: Please enter in a valid date. DateTo POST view.");
                ModelState.AddModelError("DateTo", "Please enter in a valid date");
                return View(model);
            }

            // Then Check if Date To is less than Date From
            if (model.DateTo < model.DateFrom)
            {
                ModelState.AddModelError("DateTo", "Date To cannot be less than Date From");
                return View(model);
            }

            return RedirectToAction("RequestClosure", "Closure", new { dateFrom = model.DateFrom, dateTo = model.DateTo });
        }

        [HttpGet]
        public async Task<IActionResult> RequestClosure(DateTime dateFrom, DateTime? dateTo)
        {
            RequestClosureViewModel model = new RequestClosureViewModel();
            model.TitleTagName = "Request exceptional closure";
            model.DateFrom = dateFrom;

            if (dateTo.HasValue)
            {
                model.DateTo = dateTo;
            }

            model.ReasonTypeList = await GetReasonTypes();

            model.InstitutionName = GetInstitutionName();

            LogAudit("opened RequestClosure GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestClosure(RequestClosureViewModel model)
        {
            model.TitleTagName = "Request exceptional closure";

            if (!ModelState.IsValid)
            {
                model.ErrorClass = "govuk-form-group--error";
                model.ReasonTypeList = await GetReasonTypes();

                return View(model);
            }

            // Add code when Database is done to actually save data.
            var reasonDto = _mapper.Map<ClosureReasonDto>(model);

            reasonDto.ApprovalTypeId = reasonDto.ReasonTypeId == (int)OtherReasonType.Other ? (int)ApprovalType.ApprovalRequired : (int)ApprovalType.PreApproved;

            if (reasonDto.ReasonTypeId != (int)OtherReasonType.Other)
            {
                reasonDto.Approved = true;
                reasonDto.ApprovalDate = DateTime.Now;
            }

            reasonDto.DateFrom = model.DateFrom;

            if (model.DateTo.HasValue)
            {
                reasonDto.DateTo = model.DateTo;
            }

            reasonDto.DateCreated = DateTime.Now;

            var createClosureReason = await _mediator.Send(new CreateClosureReasonCommand() { ClosureReasonDto = reasonDto });

            if (createClosureReason.IsFailure)
            {
                model.ReasonTypeList = await GetReasonTypes();
                return View(model);
            }

            if (reasonDto.ReasonTypeId != (int)OtherReasonType.Other)
            {
                await SendNotification("Thank you for your request for an exceptional closure", "Thank you for your request for an exceptional closure. \n \n The Department of Education has approved this exceptional closure and will sanction a corresponding reduction in the minimum number of days on which your school is required to be in operation during this school year.");
            }
            else
            {
                await SendNotification("Thank you for your request for an exceptional closure", "Thank you for your request for an exceptional closure. \n \n The Department of Education will be in touch in due course with the outcome.");
            }

            LogAudit("Completed RequestClosure POST view");

            return RedirectToAction("Submitted", "Closure", new { id = createClosureReason.Value });
        }

        [HttpGet]
        public async Task<IActionResult> Submitted(int id)
        {
            var getClosure = await _mediator.Send(new GetClosureReasonByIdQuery() { Id = id });

            if (getClosure.IsFailure)
            {
                return View();
            }

            var model = _mapper.Map<SubmittedViewModel>(getClosure.Value);
            model.TitleTagName = "Exceptional closure submitted";

            LogAudit("opened Submitted GET view");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var getClosure = await _mediator.Send(new GetClosureReasonByIdQuery() { Id = id });

            if (getClosure.IsFailure)
            {
                return View();
            }

            var model = _mapper.Map<EditViewModel>(getClosure.Value);
            model.TitleTagName = "Edit closure";

            model.ReasonTypeList = await GetReasonTypes();

            LogAudit("opened Edit GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ReasonTypeList = await GetReasonTypes();
                LogAudit("Error encountered: " + ModelState.IsValid.ToString() + ". Edit POST view");

                return View(model);
            }

            DateTime datefrom;

            // Check for valid dates
            if (DateTime.TryParse(CreateDate(model.DateFromYear.ToString(), model.DateFromMonth.ToString(), model.DateFromDay.ToString()), out datefrom))
            {
                model.DateFrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day);
            }
            else
            {
                model.ReasonTypeList = await GetReasonTypes();

                ModelState.AddModelError("DateFrom", "Please enter in a valid date");
                return View(model);
            }

            if (!model.IsSingleDay)
            {
                DateTime dateTo;

                // Check for valid dates
                if (DateTime.TryParse(CreateDate(model.DateToYear.ToString(), model.DateToMonth.ToString(), model.DateToDay.ToString()), out dateTo))
                {
                    model.DateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day);
                }
                else
                {
                    model.ReasonTypeList = await GetReasonTypes();
                    ModelState.AddModelError("DateTo", "Please enter in a valid date");
                    return View(model);
                }
            }

            var reasonToUpdate = _mapper.Map<ClosureReasonDto>(model);

            var updateReason = await _mediator.Send(new UpdateClosureReasonCommand() { ClosureReasonDto = reasonToUpdate });

            if (updateReason.IsFailure)
            {
                var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

                foreach (var item in getReasons.Value)
                {
                    model.ReasonTypeList.Add(item);
                }

                return View(model);
            }

            return RedirectToAction("MyClosures", "Closure");
        }

        [HttpGet]
        public async Task<IActionResult> EditReasonAsync(int id)
        {
            var getClosure = await _mediator.Send(new GetClosureReasonByIdQuery() { Id = id });

            if (getClosure.IsFailure)
            {
                return View();
            }

            var model = _mapper.Map<EditReasonViewModel>(getClosure.Value);

            model.TitleTagName = "Edit reason";

            model.ReasonTypeList = await GetReasonTypes();

            LogAudit("opened EditReason GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReason(EditReasonViewModel model)
        {
            model.TitleTagName = "Edit reason";

            if (!ModelState.IsValid)
            {
                model.ReasonTypeList = await GetReasonTypes();
                LogAudit("Error encountered: " + ModelState.IsValid.ToString() + ". EditReason POST view");

                return View(model);
            }

            var reasonToUpdate = _mapper.Map<ClosureReasonDto>(model);

            reasonToUpdate.ApprovalTypeId = reasonToUpdate.ReasonTypeId == (int)OtherReasonType.Other ? (int)ApprovalType.ApprovalRequired : (int)ApprovalType.PreApproved;

            if (reasonToUpdate.ReasonTypeId != (int)OtherReasonType.Other)
            {
                reasonToUpdate.Approved = true;
                reasonToUpdate.ApprovalDate = DateTime.Now;
            }

            var updateReason = await _mediator.Send(new UpdateClosureReasonCommand() { ClosureReasonDto = reasonToUpdate });

            if (updateReason.IsFailure)
            {
                var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

                foreach (var item in getReasons.Value)
                {
                    model.ReasonTypeList.Add(item);
                }

                return View(model);
            }

            if (reasonToUpdate.ApprovalTypeId == (int)ApprovalType.PreApproved)
            {
                return RedirectToAction("View", "Closure", new { id = model.Id });
            }

            return RedirectToAction("Edit", "Closure", new { id = model.Id });
        }

        [HttpGet]
        public async Task<IActionResult> EditDateToAsync(int id)
        {
            var getClosure = await _mediator.Send(new GetClosureReasonByIdQuery() { Id = id });

            if (getClosure.IsFailure)
            {
                return View();
            }

            var model = _mapper.Map<EditDateToViewModel>(getClosure.Value);

            model.TitleTagName = "Edit date to";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDateTo(EditDateToViewModel model)
        {
            model.TitleTagName = "Edit date to";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditDateFromAsync(int id)
        {
            var getClosure = await _mediator.Send(new GetClosureReasonByIdQuery() { Id = id });

            if (getClosure.IsFailure)
            {
                return View();
            }

            var model = _mapper.Map<EditDateFromViewModel>(getClosure.Value);

            model.TitleTagName = "Edit date from";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDateFrom(EditDateFromViewModel model)
        {
            model.TitleTagName = "Edit date from";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var getClosure = await _mediator.Send(new GetClosureReasonByIdQuery() { Id = id });

            if (getClosure.IsFailure)
            {
                return View();
            }

            var model = _mapper.Map<ViewViewModel>(getClosure.Value);

            model.TitleTagName = "View exceptional closure";

            LogAudit("opened Edit VIEW view");

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyClosures()
        {
            MyClosuresViewModel model = new MyClosuresViewModel();

            model.SectionName = "My closures";
            model.TitleTagName = "My closures";

            LogAudit("opened MyClosures view");

            var getAllClosures = await _mediator.Send(new GetAllClosuresQuery());

            if (getAllClosures.IsFailure)
            {
                return View(model);
            }

            model.ClosureList = _mapper.Map<List<ClosureReasonDto>>(getAllClosures.Value);

            return View(model);
        }

        private async Task SendNotification(string subject, string message)
        {
            var getUser = await _userManager.GetUserAsync(User);

            var getUserEmail = _signInManager.UserManager.GetEmailAsync(getUser);

            LogAudit("email sent: " + subject + ". " + message);

            _notifyService.SendEmail(getUserEmail.Result, subject, message);
        }

        public string CreateDate(string dateOfBirthYear, string dateOfBirthMonth, string dateOfBirthDay)
        {
            string dateToCheck = dateOfBirthYear + "/" + dateOfBirthMonth + "/" + dateOfBirthDay;

            return dateToCheck;
        }

        public async Task<List<ReasonTypeDto>> GetReasonTypes()
        {
            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            return getReasons.Value;
        }

        public string GetInstitutionName()
        {
            var getUser = _userManager.GetUserAsync(User);

            return getUser.Result.InstitutionName;
        }

        //[HttpGet]
        //public async Task<string> GetInstitutionName(string referenceNumber)
        //{
        //    var client = _client.CreateClient("InstitutionsClient");

        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    var result = await client.GetAsync("GetByReferenceNumber?refNumber=" + referenceNumber);

        //    Institution institution = new Institution();

        //    if (result.IsSuccessStatusCode)
        //    {
        //        using (HttpContent content = result.Content)
        //        {
        //            var resp = content.ReadAsStringAsync();
        //            institution = JsonConvert.DeserializeObject<Institution>(resp.Result);
        //        }
        //    }

        //    return institution.Name;
        //}

        internal void LogAudit(string message)
        {
            string ip = "IPAddress: " + HttpContext.Connection.RemoteIpAddress.ToString() + ", DateTime: " + DateTime.Now;

            Logger.Info(User.Identity.Name + " " + message + ". " + ip);
        }
    }
}