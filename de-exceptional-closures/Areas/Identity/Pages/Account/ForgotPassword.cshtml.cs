using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.Areas.Identity.Pages.Account
{

    [AllowAnonymous]

    public class ForgotPasswordModel : PageModel
    {
        public readonly string TitleTagName;

        public ForgotPasswordModel()
        {
            TitleTagName = "Forgot your password?";
        }

        [BindProperty]

        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public IActionResult OnGet()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}