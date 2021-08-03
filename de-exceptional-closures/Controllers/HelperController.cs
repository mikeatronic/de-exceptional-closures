using de_exceptional_closures.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    public class HelperController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public HelperController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<RedirectToActionResult> TimeoutResult()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Timeout", "Helper");
        }

        [HttpGet]
        public async Task<IActionResult> Timeout()
        {
            BaseViewModel model = new BaseViewModel();

            model.SectionName = "Time out";
            model.TitleTagName = "Timed out";

            // Logging User being directed to NIDA
            string propertiesString = "{'IPAddress': '" + HttpContext.Connection.RemoteIpAddress.ToString() + "', 'DateTime': '" + DateTime.Now + "'}";

            // Log to Nlog
            // Logger.Info(JsonConvert.SerializeObject(auditItem));

            return View(model);
        }
    }
}
