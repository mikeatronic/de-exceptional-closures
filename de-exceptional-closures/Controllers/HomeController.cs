using de_exceptional_closures.Models;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Home;
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

            model.TitleTagName = "Is the closure for a single day?";

            LogAudit("opened Is the closure for a single day GET view");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IndexViewModel model)
        {
            model.TitleTagName = "Is the closure for a single day?";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            LogAudit("opened Is the closure for a single day POST view and selected model.IsSingleDay = " + model.IsSingleDay);

            return RedirectToAction("DateFrom", "Closure", new { isSingleDay = model.IsSingleDay });
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