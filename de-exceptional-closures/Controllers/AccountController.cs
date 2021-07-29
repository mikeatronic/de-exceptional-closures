﻿using de_exceptional_closures.Config;
using de_exceptional_closures.Extensions;
using de_exceptional_closures.Notify;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Net.Http;
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

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [RateLimiting(Name = "ForgotPassword", Seconds = 5)]
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

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [RateLimiting(Name = "ForgotPasswordConfirmation", Seconds = 5)]
        public IActionResult ForgotPasswordConfirmation()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Forgot password confirmation";

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [RateLimiting(Name = "Register", Seconds = 5)]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.TitleTagName = "Register";

            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RateLimiting(Name = "Register", Seconds = 5)]
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
                   // _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    _notifyService.SendEmail(model.Email, "Confirm your email for DE exceptional closures", $"Please confirm your account by clicking this link: '{HtmlEncoder.Default.Encode(callbackUrl)}'");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Password", error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        [RateLimiting(Name = "RegisterConfirmation", Seconds = 5)]
        public IActionResult RegisterConfirmation()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Register confirmation";

            return View(model);
        }
    }
}