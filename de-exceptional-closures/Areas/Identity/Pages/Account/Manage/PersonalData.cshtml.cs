using de_exceptional_closures_infraStructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace de_exceptional_closures.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public readonly string TitleTagName;
        public readonly string SectionName;

        public PersonalDataModel(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            TitleTagName = "Delete account";
            SectionName = "Manage";
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}