using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Home
{
    public class IndexViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Please enter if approval is needed")]
        public bool? IsPreApproved { get; set; }
    }
}