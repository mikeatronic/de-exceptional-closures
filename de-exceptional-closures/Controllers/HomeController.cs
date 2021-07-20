﻿using de_exceptional_closures.Models;
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

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Accessibility()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cookies()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
