using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Account
{
    public class ForgotPassWordViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Enter an email")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }
    }
}