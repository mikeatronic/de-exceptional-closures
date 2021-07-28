using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Account
{
    public class ForgotPassWordViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}