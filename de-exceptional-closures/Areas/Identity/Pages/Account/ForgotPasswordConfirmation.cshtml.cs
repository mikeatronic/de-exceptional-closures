using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace de_exceptional_closures.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordConfirmation : PageModel
    {
        public readonly string TitleTagName;

        public ForgotPasswordConfirmation()
        {
            TitleTagName = "Forgot password confirmation";
        }
    }
}