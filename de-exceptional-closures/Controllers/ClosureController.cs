using AutoMapper;
using de_exceptional_closures.Notify;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Closure;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Features.ClosureReason.Commands;
using de_exceptional_closures_infraStructure.Features.ClosureReason.Queries;
using de_exceptional_closures_infraStructure.Features.ReasonType.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class ClosureController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly INotifyService _notifyService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private static readonly NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public ClosureController(IMediator mediator, IMapper mapper, INotifyService notifyService, SignInManager<IdentityUser> signInManager,
             UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _notifyService = notifyService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult DayType(int approvalType)
        {
            DayTypeViewModel model = new DayTypeViewModel();
            model.TitleTagName = "Is the closure for a single day?";
            model.ApprovalType = approvalType;

            LogAudit("opened Is the closure for a single day GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DayType(DayTypeViewModel model)
        {
            model.TitleTagName = "Is the closure for a single day?";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            LogAudit("opened Is the closure for a single day POST view");

            if (model.ApprovalType == (int)ApprovalType.PreApproved && model.IsSingleDay.Value)
            {
                return RedirectToAction("PreApproved", "Closure", new { isSingleDay = true });
            }
            else if (model.ApprovalType == (int)ApprovalType.PreApproved && !model.IsSingleDay.Value)
            {
                return RedirectToAction("PreApproved", "Closure", new { isSingleDay = false });
            }

            if (model.ApprovalType == (int)ApprovalType.ApprovalRequired && model.IsSingleDay.Value)
            {
                return RedirectToAction("ApprovalRequired", "Closure", new { isSingleDay = true });
            }
            else if (model.ApprovalType == (int)ApprovalType.ApprovalRequired && !model.IsSingleDay.Value)
            {
                return RedirectToAction("ApprovalRequired", "Closure", new { isSingleDay = false });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> PreApproved(bool isSingleDay)
        {
            PreApprovedViewModel model = new PreApprovedViewModel();
            model.IsSingleDay = isSingleDay;
            model.TitleTagName = "Pre-approved exceptional closure";

            model.ReasonTypeList = await GetReasonTypes();

            LogAudit("opened Pre-approved exceptional closure GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PreApproved(PreApprovedViewModel model)
        {
            model.TitleTagName = "Pre-approved exceptional closure";

            if (!ModelState.IsValid)
            {
                model.ReasonTypeList = await GetReasonTypes();

                return View(model);
            }

            LogAudit("opened Pre-approved exceptional closure POST view");

            DateTime datefrom;

            // Check for valid dates
            if (DateTime.TryParse(CreateDate(model.DateFromYear.ToString(), model.DateFromMonth.ToString(), model.DateFromDay.ToString()), out datefrom))
            {
                model.DateFrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day);
            }
            else
            {
                model.ReasonTypeList = await GetReasonTypes();

                LogAudit("Encountered an error: Please enter in a valid date. Pre-approved exceptional closure POST view.");
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
                    LogAudit("Encountered an error: Please enter in a valid date. Pre-approved exceptional closure POST view.");
                    ModelState.AddModelError("DateTo", "Please enter in a valid date");
                    return View(model);
                }
            }

            // Add code when Database is done to actually save data.
            var reasonDto = _mapper.Map<ClosureReasonDto>(model);
            reasonDto.ApprovalTypeId = (int)ApprovalType.PreApproved;
            reasonDto.DateCreated = DateTime.Now;
            reasonDto.Approved = true;
            reasonDto.ApprovalDate = DateTime.Now;

            var createClosureReason = await _mediator.Send(new CreateClosureReasonCommand() { ClosureReasonDto = reasonDto });

            if (createClosureReason.IsFailure)
            {
                model.ReasonTypeList = await GetReasonTypes();
                return View(model);
            }

            await SendNotification("Thank you for your request for an exceptional closure", "Thank you for your request for an exceptional closure. \n \n The Department of Education has approved this exceptional closure and will sanction a corresponding reduction in the minimum number of days on which your school is required to be in operation during this school year.");

            LogAudit("Completed Pre-approved exceptional closure POST view");

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
        public async Task<IActionResult> ApprovalRequired(bool isSingleDay)
        {
            ApprovalRequiredViewModel model = new ApprovalRequiredViewModel();
            model.IsSingleDay = isSingleDay;
            model.TitleTagName = "Approval required exceptional closure";

            model.ReasonTypeList = await GetReasonTypes();

            LogAudit("opened ApprovalRequired GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovalRequired(ApprovalRequiredViewModel model)
        {
            model.TitleTagName = "Approval required exceptional closure";

            if (!ModelState.IsValid)
            {
                model.ReasonTypeList = await GetReasonTypes();
                LogAudit("Error encountered: " + ModelState.IsValid.ToString() + ". ApprovalRequired POST view");
                return View(model);
            }

            DateTime deceasedDob;

            // Check for valid dates
            if (DateTime.TryParse(CreateDate(model.DateFromYear.ToString(), model.DateFromMonth.ToString(), model.DateFromDay.ToString()), out deceasedDob))
            {
                model.DateFrom = new DateTime(deceasedDob.Year, deceasedDob.Month, deceasedDob.Day);
            }
            else
            {
                model.ReasonTypeList = await GetReasonTypes();
                LogAudit("Error encountered: Please enter in a valid date. ApprovalRequired POST view");
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
                    LogAudit("Error encountered: Please enter in a valid date. ApprovalRequired POST view");
                    ModelState.AddModelError("DateTo", "Please enter in a valid date");
                    return View(model);
                }
            }

            // Add code when Database is done to actually save data.
            var reasonDto = _mapper.Map<ClosureReasonDto>(model);
            reasonDto.ApprovalTypeId = (int)ApprovalType.ApprovalRequired;
            reasonDto.DateCreated = DateTime.Now;

            var createClosureReason = await _mediator.Send(new CreateClosureReasonCommand() { ClosureReasonDto = reasonDto });

            if (createClosureReason.IsFailure)
            {
                model.ReasonTypeList = await GetReasonTypes();
                return View(model);
            }

            await SendNotification("Thank you for your request for an exceptional closure", "Thank you for your request for an exceptional closure. \n \n The Department of Education will be in touch in due course with the outcome.");

            LogAudit("Completed ApprovalRequired POST view");

            return RedirectToAction("Submitted", "Closure", new { id = createClosureReason.Value });
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

            if (!getClosure.Value.DateTo.HasValue)
            {
                model.IsSingleDay = true;
            }

            model.ReasonTypeList = await GetReasonTypes();

            LogAudit("opened Edit GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(EditViewModel model)
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
        public IActionResult CheckAnswers()
        {
            LogAudit("opened CheckAnswers view");

            return View();
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

        internal void LogAudit(string message)
        {
            string ip = "IPAddress: " + HttpContext.Connection.RemoteIpAddress.ToString() + ", DateTime: " + DateTime.Now;

            Logger.Info(User.Identity.Name + " " + message + ". " + ip);
        }
    }
}