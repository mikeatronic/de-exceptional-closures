using de_exceptional_closures.ViewModels;
using de_exceptional_closures_infraStructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    public class HelperController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private static readonly NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public HelperController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public RedirectToActionResult TimeoutResult()
        {
            return RedirectToAction("Timeout", "Helper");
        }

        [HttpGet]
        public async Task<IActionResult> Timeout()
        {
            BaseViewModel model = new BaseViewModel();

            model.SectionName = "Time out";
            model.TitleTagName = "Timed out";

            LogAudit("Logging out user: " + User.Identity.Name);

            await _signInManager.SignOutAsync();

            return View(model);
        }

        internal void LogAudit(string message)
        {
            string ip = "IPAddress: " + HttpContext.Connection.RemoteIpAddress.ToString() + ", DateTime: " + DateTime.Now;

            Logger.Info(message + ". " + ip);
        }
    }
}