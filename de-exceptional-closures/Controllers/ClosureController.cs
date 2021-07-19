using AutoMapper;
using de_exceptional_closures.ViewModels;
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

        public ClosureController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> PreApprovedAsync()
        {
            PreApprovedViewModel model = new PreApprovedViewModel();

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
        public async System.Threading.Tasks.Task<IActionResult> PreApprovedAsync(PreApprovedViewModel model)
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

            // Add code when Database is done to actually save data.
            var reasonDto = _mapper.Map<ClosureReasonDto>(model);
            reasonDto.ApprovalTypeId = (int)ApprovalType.PreApproved;
            reasonDto.DateCreated = DateTime.Now;

            var createClosureReason = await _mediator.Send(new CreateClosureReasonCommand() { ClosureReasonDto = reasonDto });

            if (createClosureReason.IsFailure)
            {
                return View(model);
            }

            return RedirectToAction("Submitted");
        }

        [HttpGet]
        public IActionResult Submitted()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ApprovalRequiredAsync()
        {
            ApprovalRequiredViewModel model = new ApprovalRequiredViewModel();

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
        public async Task<IActionResult> ApprovalRequiredAsync(ApprovalRequiredViewModel model)
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

            // Add code when Database is done to actually save data.
            var reasonDto = _mapper.Map<ClosureReasonDto>(model);
            reasonDto.ApprovalTypeId = (int)ApprovalType.ApprovalRequired;
            reasonDto.DateCreated = DateTime.Now;

            var createClosureReason = await _mediator.Send(new CreateClosureReasonCommand() { ClosureReasonDto = reasonDto });

            if (createClosureReason.IsFailure)
            {
                return View(model);
            }

            return RedirectToAction("Submitted");
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
