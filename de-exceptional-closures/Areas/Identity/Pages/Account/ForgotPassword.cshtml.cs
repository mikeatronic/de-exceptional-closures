using de_exceptional_closures.Captcha;
using de_exceptional_closures.Notify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace de_exceptional_closures.Areas.Identity.Pages.Account
{

    [AllowAnonymous]

    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotifyService _notifyService;
        public readonly string TitleTagName;
        private readonly ReCaptcha _captcha;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, INotifyService notifyService, ReCaptcha captcha)
        {
            _userManager = userManager;
            _notifyService = notifyService;
            TitleTagName = "Forgot your password?";
            _captcha = captcha;
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                //if (!Request.Form.ContainsKey("g-recaptcha-response"))
                //{
                //    ModelState.AddModelError("Input.Email", "Captcha response invalid.");

                //    return Page();
                //}

                //var captcha = Request.Form["g-recaptcha-response"].ToString();

                //if (!await _captcha.IsValid(captcha))
                //{
                //    ModelState.AddModelError("Input.Email", "You must pass the captcha check to continue.");

                //    return Page();
                //}

                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await _notifyService.SendEmail(Input.Email, "Reset Password", $"Please reset your password by clicking this link \n \n {HtmlEncoder.Default.Encode(callbackUrl)}");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
