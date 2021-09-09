using AutoMapper;
using de_exceptional_closures.Models;
using de_exceptional_closures.Notify;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Home;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Data;
using de_exceptional_closures_infraStructure.Features.AdminApprovalList.Queries;
using de_exceptional_closures_infraStructure.Features.AutoApprovalList.Queries;
using de_exceptional_closures_infraStructure.Features.ClosureReason.Commands;
using de_exceptional_closures_infraStructure.Features.ClosureReason.Queries;
using de_exceptional_closures_infraStructure.Features.ReasonType.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpClientFactory _client;
        private readonly IMapper _mapper;
        private readonly INotifyService _notifyService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private static readonly NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public HomeController(IMediator mediator, UserManager<ApplicationUser> userManager, IHttpClientFactory client, IMapper mapper, INotifyService notifyService, SignInManager<ApplicationUser> signInManager)
        {
            _mediator = mediator;
            _userManager = userManager;
            _client = client;
            _notifyService = notifyService;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.SectionName = "Home";

            model.TitleTagName = "Create closure";
            model.InstitutionName = await GetInstitution();
            model.ReasonTypeList = await GetReasonTypes();

            LogAudit("opened Is the closure for a single day GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            model.TitleTagName = "Create closure";

            if (!ModelState.IsValid)
            {
                model.ReasonTypeList = await GetReasonTypes();

                return View(model);
            }

            if (model.IsSingleDay.Value)
            {
                DateTime datefrom;

                // Check for valid dates
                if (DateTime.TryParse(CreateDate(model.DateFromYear.ToString(), model.DateFromMonth.ToString(), model.DateFromDay.ToString()), out datefrom))
                {
                    model.DateFrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day);
                }
                else
                {
                    LogAudit("Encountered an error: Please enter in a valid date. DateFrom POST view.");
                    ModelState.AddModelError("DateFromDay", "Please enter in a valid date");

                    model.ReasonTypeList = await GetReasonTypes();

                    return View(model);
                }

                // Then Check if the retrospective period is greater than 14 days
                if ((DateTime.Now - model.DateFrom).TotalDays > 365)
                {
                    ModelState.AddModelError("DateFromDay", "Retrospective closures cannot be more than 365 days in the past. Please contact attendance@education-ni.gov.uk for further advice if necessary");
                    model.ReasonTypeList = await GetReasonTypes();
                    return View(model);
                }
            }

            if (!model.IsSingleDay.Value)
            {
                DateTime datefrom;

                // Check for valid dates
                if (DateTime.TryParse(CreateDate(model.DateMultipleFromYear.ToString(), model.DateMultipleFromMonth.ToString(), model.DateMultipleFromDay.ToString()), out datefrom))
                {
                    model.DateMultipleFrom = new DateTime(datefrom.Year, datefrom.Month, datefrom.Day);
                }
                else
                {
                    LogAudit("Encountered an error: Please enter in a valid date. DateFrom POST view.");
                    ModelState.AddModelError("DateMultipleFromDay", "Please enter in a valid date");

                    model.ReasonTypeList = await GetReasonTypes();
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
                    ModelState.AddModelError("DateToDay", "Please enter in a valid date");
                    model.ReasonTypeList = await GetReasonTypes();
                    return View(model);
                }

                // Then Check if Date To is less than Date From
                if (model.DateTo < model.DateMultipleFrom)
                {
                    ModelState.AddModelError("DateToDay", "Date To cannot be less than Date From");
                    model.ReasonTypeList = await GetReasonTypes();
                    return View(model);
                }

                // Then Check if the period is greater than 30 days
                if ((model.DateTo - model.DateMultipleFrom).Value.TotalDays > 30)
                {
                    ModelState.AddModelError("DateToDay", "Closure cannot be more than 30 days maximum");
                    model.ReasonTypeList = await GetReasonTypes();
                    return View(model);
                }

                // Then Check if the retrospective period is greater than 365 days
                if ((DateTime.Now - model.DateMultipleFrom).TotalDays > 365)
                {
                    ModelState.AddModelError("DateMultipleFromDay", "Retrospective closures cannot be more than 365 days in the past. Please contact attendance@education-ni.gov.uk for further advice if necessary");
                    model.ReasonTypeList = await GetReasonTypes();
                    return View(model);
                }
            }



            // Set Approval type
            model.ApprovalTypeId = await GetApprovalType(model.ReasonTypeId);

            // Reset Covid questions
            if (model.ReasonTypeId != (int)OtherReasonType.Covid)
            {
                model.CovidQ1 = null;
                model.CovidQ2 = null;
                model.CovidQ3 = null;
                model.CovidQ4 = null;
                model.CovidQ5 = null;
            }

            // Reset Other text
            if (model.ReasonTypeId != (int)OtherReasonType.Other)
            {
                model.OtherReason = null;
            }

            // Add code when Database is done to actually save data
            var reasonDto = _mapper.Map<ClosureReasonDto>(model);

            if (reasonDto.ApprovalTypeId == (int)ApprovalType.PreApproved)
            {
                reasonDto.Approved = true;
                reasonDto.ApprovalDate = DateTime.Now;
            }

            reasonDto.DateFrom = model.DateFrom;

            if (model.DateTo.HasValue)
            {
                reasonDto.DateTo = model.DateTo;
                reasonDto.DateFrom = model.DateMultipleFrom;
            }

            reasonDto.DateCreated = DateTime.Now;
            reasonDto.Srn = "4011654";
            // Check overlapping dates

            // bool overlap = a.start < b.end && b.start < a.end;

            var getOverlaps = await _mediator.Send(new GetOverlappingClosuresQuery() { ClosureReasonDto = reasonDto });

            if (getOverlaps.Value)
            {
                ModelState.AddModelError("DateFromDay", "This date overlaps with other closures");
                model.ReasonTypeList = await GetReasonTypes();
                return View(model);
            }

            var createClosureReason = await _mediator.Send(new CreateClosureReasonCommand() { ClosureReasonDto = reasonDto });

            if (createClosureReason.IsFailure)
            {
                return View(model);
            }

            var getNewClosureDetails = await _mediator.Send(new GetClosureReasonByIdQuery() { Id = createClosureReason.Value });

            if (reasonDto.ApprovalTypeId == (int)ApprovalType.PreApproved)
            {
                await SendApprovalNotRequiredNotification(getNewClosureDetails.Value);
            }
            else
            {
                await SendApprovalRequiredNotification(getNewClosureDetails.Value);
            }

            LogAudit("opened Is the closure for a single day POST view and selected model.IsSingleDay = ");

            return RedirectToAction("Submitted", "Closure", new { id = createClosureReason.Value });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Privacy()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Privacy";

            LogAudit("opened Privacy GET view");

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Accessibility()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Accessibility";

            LogAudit("opened Accessibility GET view");

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Cookies()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Cookies";

            LogAudit("opened Cookies GET view");

            return View(model);
        }

        private async Task<string> GetInstitution()
        {
            var client = _client.CreateClient("InstitutionsClient");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.GetAsync("GetByReferenceNumber?refNumber=" + GetInstitutionRef());

            Institution institution;

            if (result.IsSuccessStatusCode)
            {
                using (HttpContent content = result.Content)
                {
                    var resp = content.ReadAsStringAsync();
                    institution = JsonConvert.DeserializeObject<Institution>(resp.Result);
                }
            }
            else
            {
                return string.Empty;
            }

            return $"{institution.Name}, {institution.address.address1}, {institution.address.townCity}, {institution.address.postCode}";
        }

        private string GetDateFrom(DateTime? dateTo)
        {
            if (dateTo.HasValue)
            {
                return " to " + dateTo.Value.ToShortDateString();
            }

            return string.Empty;
        }

        private async Task SendCitizenEmailAsync(string subject)
        {
            var getUser = await _userManager.GetUserAsync(User);

            var getUserEmail = _signInManager.UserManager.GetEmailAsync(getUser);

            await _notifyService.SendEmailAsync(getUserEmail.Result, "Request for exceptional closure", subject);
        }

        private async Task SendApprovalRequiredNotification(ClosureReasonDto reasonDto)
        {
            // Send email first to the citizen
            await SendCitizenEmailAsync("Thank you for your request for an exceptional closure. \n \n The Department of Education will be in touch in due course with the outcome.");

            // Then send an email to the other parties
            string msg = reasonDto.InstitutionName + " (" + reasonDto.Srn + ") has requested an exceptional closure. \n \n The school closed on or will close on " + reasonDto.DateFrom.Value.ToShortDateString() + GetDateFrom(reasonDto.DateTo) + " because of " + reasonDto.ReasonType + ". \n \n Approval is required for this closure.";

            // Get list of approvers to email them the request
            var getApprovers = await _mediator.Send(new GetAllApproversQuery());

            foreach (var item in getApprovers.Value)
            {
                await _notifyService.SendEmailAsync(item.Email, "Request for exceptional closure - approval required", msg);
            }
        }

        private async Task SendApprovalNotRequiredNotification(ClosureReasonDto reasonDto)
        {
            // Send email first to the citizen
            await SendCitizenEmailAsync(reasonDto.InstitutionName + " has requested an exceptional closure. The school closed on or will close " + reasonDto.DateFrom.Value.ToShortDateString() + "  because of " + reasonDto.ReasonType + ". \n \n The request has been approved.");

            // Then send an email to the other parties
            string subject = reasonDto.InstitutionName + " (" + reasonDto.Srn + ") has requested an exceptional closure. \n \n The school closed on or will close " + reasonDto.DateFrom.Value.ToShortDateString() + GetDateFrom(reasonDto.DateTo) + " because of " + reasonDto.ReasonType + ". \n \n The request has been approved.";

            // Get AutoApprover list
            var getApprovers = await _mediator.Send(new GetAllApprovalListQuery());

            foreach (var item in getApprovers.Value)
            {
                await _notifyService.SendEmailAsync(item.Email, "Request for exceptional closure", subject);
            }
        }

        private string GetInstitutionRef()
        {
            var getUser = _userManager.GetUserAsync(User);

            return getUser.Result.InstitutionReference;
        }

        private async Task<List<ReasonTypeDto>> GetReasonTypes()
        {
            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            return getReasons.Value;
        }

        private async Task<int> GetApprovalType(int id)
        {
            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            if (getReasons.Value.SingleOrDefault(i => i.Id == id).ApprovalRequired.HasValue && getReasons.Value.SingleOrDefault(i => i.Id == id).ApprovalRequired.Value)
            {
                return (int)ApprovalType.ApprovalRequired;
            }

            return (int)ApprovalType.PreApproved;
        }

        private string CreateDate(string dateOfBirthYear, string dateOfBirthMonth, string dateOfBirthDay)
        {
            string dateToCheck = dateOfBirthYear + "/" + dateOfBirthMonth + "/" + dateOfBirthDay;

            return dateToCheck;
        }

        private void LogAudit(string message)
        {
            string ip = "IPAddress: " + HttpContext.Connection.RemoteIpAddress.ToString() + ", DateTime: " + DateTime.Now;

            Logger.Info(User.Identity.Name + " " + message + ". " + ip);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}