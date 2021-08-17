using de_exceptional_closures_infraStructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace de_exceptional_closures.Areas.Identity.Pages.Account.Manage
{
    public class InstitutionModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public readonly string TitleTagName;
        public readonly string SectionName;

        public InstitutionModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            TitleTagName = "Manage school";
            SectionName = "Manage";
        }

        public string Username { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please enter a school")]
            [Display(Name = "School")]
            public string InstitutionName { get; set; }

            [Required(ErrorMessage = "Please enter a school reference")]
            [Display(Name = "Reference")]
            public string InstitutionReference { get; set; }

            public string Search { get; set; }
            public string SearchInstitutions { get; set; }
        }

        private void Load(ApplicationUser user)
        {
            Input = new InputModel
            {
                InstitutionName = user.InstitutionName,
                InstitutionReference = user.InstitutionReference
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            user.InstitutionName = Input.InstitutionName;
            user.InstitutionReference = Input.InstitutionReference;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to manage school.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your school has been updated";
            return RedirectToPage();
        }
    }
}