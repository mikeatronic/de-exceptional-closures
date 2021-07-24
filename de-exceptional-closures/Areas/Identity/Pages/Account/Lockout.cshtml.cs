using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace de_exceptional_closures.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
        public readonly string TitleTagName;

        public LockoutModel()
        {
            TitleTagName = "Locked out";
        }
        public void OnGet()
        {

        }
    }
}