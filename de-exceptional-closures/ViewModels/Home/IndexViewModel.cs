using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Home
{
    public class IndexViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Please enter if the closure is for a single day or not")]
        public bool? IsSingleDay { get; set; }
    }
}