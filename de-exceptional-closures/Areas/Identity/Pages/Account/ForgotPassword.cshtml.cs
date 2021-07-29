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