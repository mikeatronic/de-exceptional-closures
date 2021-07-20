using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Closure
{
    public class DayTypeViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Please enter if the closure is for a single day or not")]
        public bool IsSingleDay { get; set; }
        public int ApprovalType { get; set; }
    }
}