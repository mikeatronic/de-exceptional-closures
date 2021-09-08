using AutoMapper;
using de_exceptional_closures.Extensions;
using de_exceptional_closures.Notify;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Closure;
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
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> CheckAnswersAsync()
        {
            CheckAnswersViewModel model = new CheckAnswersViewModel();

            var mod = HttpContext.Session.Get<IndexViewModel>("CreateClosureObj");

            model.InstitutionName = mod.InstitutionName;
            model.OtherReason = mod.OtherReason;

            if (mod.IsSingleDay.Value)
            {
                model.DateFrom = mod.DateFrom;
            }
            else
            {
                model.DateFrom = mod.DateMultipleFrom;
            }

            model.DateTo = mod.DateTo;
            model.OtherReason = mod.OtherReason;
            model.ReasonType = await GetReasonTypeAsync(mod.ReasonTypeId);
            model.ApprovalTypeId = mod.ApprovalTypeId;
            model.CovidQ1 = mod.CovidQ1;
            model.CovidQ2 = mod.CovidQ2;
            model.CovidQ3 = mod.CovidQ3;
            model.CovidQ4 = mod.CovidQ4;
            model.CovidQ5 = mod.CovidQ5;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckAnswers()
        {
            var model = HttpContext.Session.Get<IndexViewModel>("CreateClosureObj");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Add code when Database is done to actually save data
            var reasonDto = _mapper.Map<ClosureReasonDto>(model);

            reasonDto.ApprovalTypeId = await GetApprovalType(model.ReasonTypeId);

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

            LogAudit("Completed RequestClosure POST view");

            // Reset session variable
            HttpContext.Session.Remove("CreateClosureObj");

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

            if (model.ApprovalTypeId == 1)
            {
                model.TitleTagName = "Exceptional closure approved";
            }
            else
            {
                model.TitleTagName = "Exceptional closure submitted";
            }

            LogAudit("opened Submitted GET view");

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

        private async Task<int> GetApprovalType(int id)
        {
            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            if (getReasons.Value.SingleOrDefault(i => i.Id == id).ApprovalRequired.HasValue && getReasons.Value.SingleOrDefault(i => i.Id == id).ApprovalRequired.Value)
            {
                return (int)ApprovalType.ApprovalRequired;
            }

            return (int)ApprovalType.PreApproved;
        }

        private async Task<string> GetReasonTypeAsync(int id)
        {
            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            return getReasons.Value.SingleOrDefault(i => i.Id == id).Description;
        }

        private void LogAudit(string message)
        {
            string ip = "IPAddress: " + HttpContext.Connection.RemoteIpAddress.ToString() + ", DateTime: " + DateTime.Now;

            Logger.Info(User.Identity.Name + " " + message + ". " + ip);
        }
    }
}