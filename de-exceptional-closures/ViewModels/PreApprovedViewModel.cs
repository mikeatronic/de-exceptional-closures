using de_exceptional_closures_core.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels
{
    public class PreApprovedViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date from is required")]
        [Display(Name = "Date from")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Gets or sets "Date of death Day.
        /// </summary>
        [Required(ErrorMessage = "Date from day is required")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromDay { get; set; }

        /// <summary>
        /// Gets or sets "Date of death Month.
        /// </summary>
        [Required(ErrorMessage = "Date from month is required")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromMonth { get; set; }

        /// <summary>
        /// Gets or sets "Date of death Year.
        /// </summary>
        [Required(ErrorMessage = "Date from year is required")]
        [Display(Name = "Year")]
        [Range(2000, 2100, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromYear { get; set; }

        public DateTime DateTo { get; set; }
        [Required(ErrorMessage = "Reason type required")]
        public int ReasonTypeId { get; set; }

        [Display(Name = "Other reason")]
        public string OtherReason { get; set; }
        public ApprovalType ApprovalType => ApprovalType.PreApproved;
    }
}