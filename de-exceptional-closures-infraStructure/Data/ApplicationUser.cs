using Microsoft.AspNetCore.Identity;

namespace de_exceptional_closures_infraStructure.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string InstitutionReference { get; set; }
        public string InstitutionName { get; set; }
    }
}