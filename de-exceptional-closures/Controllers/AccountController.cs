using de_exceptional_closures.Config;
using de_exceptional_closures.Extensions;
using de_exceptional_closures.Notify;
using de_exceptional_closures.ViewModels;
using de_exceptional_closures.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
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
        public readonly string TitleTagName;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<CaptchaConfig> _captchaConfig;


        public AccountController(UserManager<IdentityUser> userManager, INotifyService notifyService, IHttpClientFactory httpClientFactory, IOptions<CaptchaConfig> captchaConfig)
        {
            _userManager = userManager;
            _notifyService = notifyService;
            _httpClientFactory = httpClientFactory;
            _captchaConfig = captchaConfig;
            TitleTagName = "Forgot your password?";
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordAsync()
        {
            ForgotPassWordViewModel model = new ForgotPassWordViewModel();
            model.TitleTagName = "Forgot your password?";

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [RateLimiting(Name = "ForgotPasswordConfirmation", Seconds = 2)]
        public IActionResult ForgotPasswordConfirmation()
        {
            BaseViewModel model = new BaseViewModel();
            model.TitleTagName = "Forgot password confirmation";

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [RateLimiting(Name = "ForgotPassword", Seconds = 2)]
        public async Task<IActionResult> ForgotPassword(ForgotPassWordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!Request.Form.ContainsKey("g-recaptcha-response"))
                {
                    ModelState.AddModelError("Email", "Captcha response invalid.");

                    return View(model);
                }

                var captcha = Request.Form["g-recaptcha-response"].ToString();

                if (!await IsValid(captcha))
                {
                    ModelState.AddModelError("Email", "You must pass the captcha check to continue.");

                    return View(model);
                }

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

        public async Task<bool> IsValid(string captcha)
        {
            var client = _httpClientFactory.CreateClient("CaptchaClient");

            var postTask = await client
                    .PostAsync($"?secret={_captchaConfig.Value.Secret}&response={captcha}", new StringContent(""));

            var result = await postTask.Content.ReadAsStringAsync();
            var resultObject = JObject.Parse(result);
            dynamic success = resultObject["success"];
            return (bool)success;
        }
    }
}