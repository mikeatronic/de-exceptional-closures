using de_exceptional_closures.Extensions;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Home
{
    public class IndexViewModel : BaseViewModel
    {
        public IndexViewModel()
        {
            ReasonTypeList = new List<ReasonTypeDto>();
        }
        public int Id { get; set; }
        public int ApprovalTypeId { get; set; }

        [RequiredIf("IsSingleDay", true, ErrorMessage = "Please enter Date from")]
        [Display(Name = "Date from")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Date to")]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [RequiredIf("IsSingleDay", true, ErrorMessage = "Please enter Date from day")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromDay { get; set; }

        [RequiredIf("IsSingleDay", true, ErrorMessage = "Please enter Date from month")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromMonth { get; set; }

        [RequiredIf("IsSingleDay", true, ErrorMessage = "Please enter Date from year")]
        [Display(Name = "Year")]
        [Range(2000, 2100, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateFromYear { get; set; }

        [RequiredIf("IsSingleDay", false, ErrorMessage = "Please enter Date from")]
        [Display(Name = "Date from")]
        [DataType(DataType.Date)]
        public DateTime DateMultipleFrom { get; set; }

        [RequiredIf("IsSingleDay", false, ErrorMessage = "Please enter Date from day")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateMultipleFromDay { get; set; }

        [RequiredIf("IsSingleDay", false, ErrorMessage = "Please enter Date from month")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateMultipleFromMonth { get; set; }

        [RequiredIf("IsSingleDay", false, ErrorMessage = "Please enter date from year")]
        [Display(Name = "Year")]
        [Range(2000, 2100, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateMultipleFromYear { get; set; }

        [Display(Name = "Day")]
        [RequiredIf("IsSingleDay", false, ErrorMessage = "Please enter Day To")]
        [Range(1, 31, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateToDay { get; set; }

        [Display(Name = "Month")]
        [RequiredIf("IsSingleDay", false, ErrorMessage = "Please enter Month To")]
        [Range(1, 12, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateToMonth { get; set; }

        [Display(Name = "Year")]
        [RequiredIf("IsSingleDay", false, ErrorMessage = "Please enter Year To")]
        [Range(2000, 2100, ErrorMessage = "{0} must be between {1} and {2}")]
        public int? DateToYear { get; set; }

        [Required(ErrorMessage = "Reason type required")]
        public int ReasonTypeId { get; set; }

        [Display(Name = "Other reason")]
        [MinLength(5, ErrorMessage = "Reason must be at least 5 characters long")]
        [MaxLength(1024, ErrorMessage = "Only 1024 characters are allowed")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Other, ErrorMessage = "Please enter other reason")]
        [AlphaNumericLimitedSpecialChars]
        public string OtherReason { get; set; }

        [Display(Name = "Covid - Other reason")]
        [MinLength(5, ErrorMessage = "Reason must be at least 5 characters long")]
        [MaxLength(1024, ErrorMessage = "Only 1024 characters are allowed")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Covid, ErrorMessage = "Please enter Covid other reason")]
        [AlphaNumericLimitedSpecialChars]
        public string OtherReasonCovid { get; set; }

        [Required(ErrorMessage = "Please enter a day type")]
        public bool? IsSingleDay { get; set; }
        public List<ReasonTypeDto> ReasonTypeList { get; set; }
        public string InstitutionName { get; set; }
    }
}