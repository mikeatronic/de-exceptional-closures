using de_exceptional_closures.Models;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Home;
using de_exceptional_closures_core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static readonly NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.TitleTagName = "Is your exceptional closure pre-approved?";

            LogAudit("opened Is your exceptional closure pre-approved GET view");

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

            if (model.IsPreApproved.Value)
            {
                LogAudit("opened Is your exceptional closure pre-approved POST view and selected model.preapproved = " + model.IsPreApproved);

                return RedirectToAction("DayType", "Closure", new { approvalType = (int)ApprovalType.PreApproved });
            }

            LogAudit("opened Is your exceptional closure pre-approved POST view and selected model.preapproved = " + model.IsPreApproved);

            return RedirectToAction("DayType", "Closure", new { approvalType = (int)ApprovalType.ApprovalRequired });
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

        internal void LogAudit(string message)
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