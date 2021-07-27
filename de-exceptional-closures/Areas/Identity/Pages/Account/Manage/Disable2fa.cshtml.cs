using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace de_exceptional_closures.Areas.Identity.Pages.Account.Manage
{
    public class Disable2faModel : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult OnGet()
        {
            return RedirectToPage("./Login");
        }
    }
}