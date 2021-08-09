using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.Areas.Identity.Pages.Account.Manage
{
    public class InstitutionModel : PageModel
    {
        public readonly string TitleTagName;
        public readonly string SectionName;

        public InstitutionModel()
        {
            TitleTagName = "Manage Institution";
            SectionName = "Manage";
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Institution")]
            public string InstitutionName { get; set; }

            [Required]
            [Display(Name = "Reference")]
            public string InstitutionReference { get; set; }

            public string Search { get; set; }
            public string SearchInstitutions { get; set; }
        }
    }
}