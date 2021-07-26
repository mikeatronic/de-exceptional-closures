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

        public ClosureController(IMediator mediator, IMapper mapper, INotifyService notifyService)
        {
            _mediator = mediator;
            _mapper = mapper;
            _notifyService = notifyService;
        }

        [HttpGet]
        public IActionResult DayType(int approvalType)
        {
            DayTypeViewModel model = new DayTypeViewModel();
            model.TitleTagName = "Is the closure for a single day?";
            model.ApprovalType = approvalType;

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

            if (model.ApprovalType == (int)ApprovalType.PreApproved && model.IsSingleDay)
            {
                return RedirectToAction("PreApproved", "Closure", new { isSingleDay = true });
            }
            else if (model.ApprovalType == (int)ApprovalType.PreApproved && !model.IsSingleDay)
            {
                return RedirectToAction("PreApproved", "Closure", new { isSingleDay = false });
            }

            if (model.ApprovalType == (int)ApprovalType.ApprovalRequired && model.IsSingleDay)
            {
                return RedirectToAction("ApprovalRequired", "Closure", new { isSingleDay = true });
            }
            else if (model.ApprovalType == (int)ApprovalType.ApprovalRequired && !model.IsSingleDay)
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

            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            if (getReasons.IsFailure)
            {
                return View(model);
            }

            foreach (var item in getReasons.Value)
            {
                model.ReasonTypeList.Add(item);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PreApproved(PreApprovedViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

                foreach (var item in getReasons.Value)
                {
                    model.ReasonTypeList.Add(item);
                }

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
                    ModelState.AddModelError("DateTo", "Please enter in a valid date");
                    return View(model);
                }
            }

            // Add code when Database is done to actually save data.
            var reasonDto = _mapper.Map<ClosureReasonDto>(model);
            reasonDto.ApprovalTypeId = (int)ApprovalType.PreApproved;
            reasonDto.DateCreated = DateTime.Now;

            var createClosureReason = await _mediator.Send(new CreateClosureReasonCommand() { ClosureReasonDto = reasonDto });

            if (createClosureReason.IsFailure)
            {
                return View(model);
            }

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

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ApprovalRequired(bool isSingleDay)
        {
            ApprovalRequiredViewModel model = new ApprovalRequiredViewModel();
            model.IsSingleDay = isSingleDay;

            var getReasons = await _mediator.Send(new GetAllReasonTypesQuery());

            if (getReasons.IsFailure)
            {
                return View(model);
            }

            foreach (var item in getReasons.Value)
            {
                model.ReasonTypeList.Add(item);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovalRequired(ApprovalRequiredViewModel model)
        {
            if (!ModelState.IsValid)
            {
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
                return View(model);
            }

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

            var model = _mapper.Map<EditViewModel>(getClosure.Value);

            return View(model);
        }

        [HttpGet]
        public IActionResult CheckAnswers()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MyClosures()
        {
            MyClosuresViewModel model = new MyClosuresViewModel();

            model.SectionName = "My closures";
            model.TitleTagName = "My closures";

            var getAllClosures = await _mediator.Send(new GetAllClosuresQuery());

            if (getAllClosures.IsFailure)
            {
                return View(model);
            }

            model.ClosureList = _mapper.Map<List<ClosureReasonDto>>(getAllClosures.Value);

            return View(model);
        }

        public string CreateDate(string dateOfBirthYear, string dateOfBirthMonth, string dateOfBirthDay)
        {
            string dateToCheck = dateOfBirthYear + "/" + dateOfBirthMonth + "/" + dateOfBirthDay;

            return dateToCheck;
        }
    }
}
