using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace de_exceptional_closures.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
    {
        public readonly string TitleTagName;
        public ResetPasswordConfirmationModel()
        {
            TitleTagName = "Reset password confirmation";
        }
    }
}