using de_exceptional_closures.Extensions;
using de_exceptional_closures.Models;
using de_exceptional_closures.Notify;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Account;
using de_exceptional_closures_infraStructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotifyService _notifyService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _client;
        public readonly string TitleTagName;
        private static readonly NLog.Logger Logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        public AccountController(UserManager<ApplicationUser> userManager, INotifyService notifyService, SignInManager<ApplicationUser> signInManager, IHttpClientFactory client)
        {
            _userManager = userManager;
            _notifyService = notifyService;
            _signInManager = signInManager;
            _client = client;
            TitleTagName = "Forgot your password?";
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ForgotPassWordViewModel model = new ForgotPassWordViewModel();
            model.TitleTagName = "Forgot password";

            Logger.Info("opened Forgot your password GET view");

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [RateLimiting(Name = "ForgotPassword", Minutes = 1)]
        public async Task<IActionResult> ForgotPassword(ForgotPassWordViewModel model)
        {
            model.TitleTagName = "Forgot password";

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

                await _notifyService.SendEmailAsync(model.Email, "Reset Password", $"Please reset your password by clicking this link \n \n {HtmlEncoder.Default.Encode(callbackUrl)}");

                Logger.Info("opened Forgot your password POST view and being re-directed to ForgotPasswordConfirmation view");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [RateLimiting(Name = "ForgotPasswordConfirmation", Minutes = 1)]
        public IActionResult ForgotPasswordConfirmation()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Forgot password confirmation";

            Logger.Info("opened Forgot password confirmation GET view");

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            model.TitleTagName = "Register";

            Logger.Info("opened Register GET view");

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [RateLimiting(Name = "Register", Minutes = 5)]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            model.TitleTagName = "Register";

            returnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var searchInstitution = await CheckInstitution(model.InstitutionReference);

            // Then check if Institute reference is valid
            if (searchInstitution == string.Empty)
            {
                Logger.Info("tried to Register with an unknown Institute: ");

                ModelState.AddModelError("InstitutionReference", "Cannot find Institute");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, InstitutionReference = model.InstitutionReference, InstitutionName = searchInstitution };

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

                    await _notifyService.SendEmailAsync(model.Email, "Reporting your school's exceptional closure", "Thank you for registering to complete exceptional closure returns for your school. This email contains further information to enable you to complete the process. " + " \n \n Clicking on the following link will enable you to confirm your email address authenticate your registration: \n \n " + $"'{HtmlEncoder.Default.Encode(callbackUrl)}'" + "\n \n When you have authenticated your account, you can log in to complete an exceptional closure return for your school. \n \n You should log in using your email address and password.");

                    Logger.Info("Email sent to confirm account");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        Logger.Info("Redirecting to RegisterConfirmation view");
                        return RedirectToAction("RegisterConfirmation", "Account");
                    }
                    else
                    {
                        Logger.Info("Signing user in");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    Logger.Info("encountered an error: " + error.Description);
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

            Logger.Info("opened Register confirmation view");

            return View(model);
        }

        [HttpGet]
        public async Task<string> CheckInstitution(string referenceNumber)
        {
            var client = _client.CreateClient("InstitutionsClient");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.GetAsync("GetSchoolByReferenceNumber?refNumber=" + referenceNumber);

            if (result.IsSuccessStatusCode)
            {
                Institution institution;

                using (HttpContent content = result.Content)
                {
                    var resp = content.ReadAsStringAsync();
                    institution = JsonConvert.DeserializeObject<Institution>(resp.Result);
                }

                return institution.Name;
            }

            return string.Empty;
        }
    }
}