using de_exceptional_closures.Extensions;
using de_exceptional_closures.Notify;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotifyService _notifyService;
        private readonly SignInManager<IdentityUser> _signInManager;
        public readonly string TitleTagName;
        private static readonly NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public AccountController(UserManager<IdentityUser> userManager, INotifyService notifyService, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _notifyService = notifyService;
            _signInManager = signInManager;
            TitleTagName = "Forgot your password?";
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ForgotPassWordViewModel model = new ForgotPassWordViewModel();
            model.TitleTagName = "Forgot your password?";

            LogAudit("opened Forgot your password GET view");

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [RateLimiting(Name = "ForgotPassword", Minutes = 15)]
        public async Task<IActionResult> ForgotPassword(ForgotPassWordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                _notifyService.SendEmail(model.Email, "Reset Password", $"Please reset your password by clicking this link \n \n {HtmlEncoder.Default.Encode(callbackUrl)}");

                LogAudit("opened Forgot your password POST view and being re-directed to ForgotPasswordConfirmation view");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [RateLimiting(Name = "ForgotPasswordConfirmation", Minutes = 15)]
        public IActionResult ForgotPasswordConfirmation()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Forgot password confirmation";

            LogAudit("opened Forgot password confirmation GET view");

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.TitleTagName = "Register";

            LogAudit("opened Register GET view");

            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RateLimiting(Name = "Register", Minutes = 15)]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model, string returnUrl = null)
        {
            model.TitleTagName = "Register";

            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    _notifyService.SendEmail(model.Email, "Confirm your email for DE exceptional closures", $"Please confirm your account by clicking this link: '{HtmlEncoder.Default.Encode(callbackUrl)}'");

                    LogAudit("Email sent to confirm account");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        LogAudit("Redirecting to RegisterConfirmation view");
                        return RedirectToAction("RegisterConfirmation", "Account");
                    }
                    else
                    {
                        LogAudit("Signing user in");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
               
                foreach (var error in result.Errors)
                {
                    LogAudit("encountered an error: " + error.Description);
                    ModelState.AddModelError("Password", error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [RateLimiting(Name = "RegisterConfirmation", Minutes = 15)]
        public IActionResult RegisterConfirmation()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Register confirmation";

            LogAudit("opened Register confirmation view");

            return View(model);
        }

        internal void LogAudit(string message)
        {
            string ip = "IPAddress: " + HttpContext.Connection.RemoteIpAddress.ToString() + ", DateTime: " + DateTime.Now;

            Logger.Info(User.Identity.Name + " " + message + ". " + ip);
        }
    }
}