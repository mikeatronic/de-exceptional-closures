using de_exceptional_closures.ViewModels;
using de_exceptional_closures_infraStructure.Features.ReasonType.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class ClosureController : Controller
    {
        private readonly IMediator _mediator;

        public ClosureController(IMediator mediator)
        {
            _mediator = mediator;
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



            return View(model);
        }

        [HttpGet]
        public IActionResult ApprovalRequired()
        {
            return View();
        }
    }
}
