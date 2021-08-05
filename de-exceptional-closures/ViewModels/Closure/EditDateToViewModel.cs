using System;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Closure
{
    public class EditDateToViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string InstitutionName { get; set; }
        [Required(ErrorMessage = "Date to is required")]
        [Display(Name = "Date to")]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }

        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateToDay { get; set; }

        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateToMonth { get; set; }

        [Display(Name = "Year")]
        [Range(2000, 2100, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateToYear { get; set; }
    }
}