using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;

namespace de_exceptional_closures.Areas.Identity.Pages.Account.Manage
{
    public class InstitutionModel : PageModel
    {
        public readonly string TitleTagName;
        public readonly string SectionName;
        private readonly IHttpClientFactory _client;

        public InstitutionModel(IHttpClientFactory client)
        {
            TitleTagName = "Manage Institution";
            SectionName = "Manage";
            _client = client;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Institution")]
            public string InstitutionName { get; set; }
        }


        public void OnGet()
        {
        }


    }
}