using System;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Closure
{
    public class EditDateFromViewModel : BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Date from is required")]
        [Display(Name = "Date from")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Required(ErrorMessage = "Date from day is required")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromDay { get; set; }

        [Required(ErrorMessage = "Date from month is required")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromMonth { get; set; }

        [Required(ErrorMessage = "Date from year is required")]
        [Display(Name = "Year")]
        [Range(2000, 2100, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromYear { get; set; }
        public string InstitutionName { get; set; }
    }
}