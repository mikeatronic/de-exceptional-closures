using de_exceptional_closures.Extensions;
using de_exceptional_closures.Models;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Home;
using de_exceptional_closures_core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.TitleTagName = "Is your exceptional closure pre-approved?";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IndexViewModel model)
        {
            model.TitleTagName = "Is your exceptional closure pre-approved?";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.IsPreApproved)
            {
                return RedirectToAction("DayType", "Closure", new { approvalType = (int)ApprovalType.PreApproved });
            }

            return RedirectToAction("DayType", "Closure", new { approvalType = (int)ApprovalType.ApprovalRequired });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Privacy()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Privacy";
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Accessibility()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Accessibility";
            return View(model);
        }

        [AllowAnonymous]
        [RateLimiting(Name = "Cookies", Seconds = 1)]
        [HttpGet]
        public IActionResult Cookies()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Cookies";
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
