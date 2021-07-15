using AutoMapper;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using de_exceptional_closures_infraStructure.Features.ClosureReason.Commands;
using de_exceptional_closures_infraStructure.Features.ReasonType.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class ClosureController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ClosureController(IMediator mediator, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _mapper = mapper;
            _userManager = userManager;
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

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            reasonDto.UserId = user.Id;

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
        public IActionResult ApprovalRequired()
        {
            return View();
        }

        public string CreateDate(string dateOfBirthYear, string dateOfBirthMonth, string dateOfBirthDay)
        {
            string dateToCheck = dateOfBirthYear + "/" + dateOfBirthMonth + "/" + dateOfBirthDay;

            return dateToCheck;
        }
    }
}
