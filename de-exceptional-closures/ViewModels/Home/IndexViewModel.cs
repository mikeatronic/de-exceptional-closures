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

        [RequiredIf("IsSingleDay", true, ErrorMessage = "Enter date closure is from")]
        [Display(Name = "Date from")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [RequiredIf("IsSingleDay", true, ErrorMessage = "Date closure from must include a day")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "Date of closure day must be between {1} and {2}")]
        public int? DateFromDay { get; set; }

        [RequiredIf("IsSingleDay", true, ErrorMessage = "Date closure from must include a month")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "Date of closure month must be between {1} and {2}")]
        public int? DateFromMonth { get; set; }

        [RequiredIf("IsSingleDay", true, ErrorMessage = "Date closure from must include a year")]
        [Display(Name = "Year")]
        [Range(2000, 2100, ErrorMessage = "Date of closure year must be between {1} and {2}")]
        public int? DateFromYear { get; set; }

        [RequiredIf("IsSingleDay", false, ErrorMessage = "Enter date closure is from")]
        [Display(Name = "Date from")]
        [DataType(DataType.Date)]
        public DateTime DateMultipleFrom { get; set; }

        [RequiredIf("IsSingleDay", false, ErrorMessage = "Date closure from must include a day")]
        [Display(Name = "Day")]
        [Range(1, 31, ErrorMessage = "Date of closure day must be between {1} and {2}")]
        public int? DateMultipleFromDay { get; set; }

        [RequiredIf("IsSingleDay", false, ErrorMessage = "Date closure from must include a month")]
        [Display(Name = "Month")]
        [Range(1, 12, ErrorMessage = "Date of closure month must be between {1} and {2}")]
        public int? DateMultipleFromMonth { get; set; }

        [RequiredIf("IsSingleDay", false, ErrorMessage = "Date closure from must include a year")]
        [Display(Name = "Year")]
        [Range(2000, 2100, ErrorMessage = "Date of closure year must be between {1} and {2}")]
        public int? DateMultipleFromYear { get; set; }

        [Display(Name = "Date to")]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

        [Display(Name = "Day")]
        [RequiredIf("IsSingleDay", false, ErrorMessage = "Date closure to must include a day")]
        [Range(1, 31, ErrorMessage = "Date of closure to day must be between {1} and {2}")]
        public int? DateToDay { get; set; }

        [Display(Name = "Month")]
        [RequiredIf("IsSingleDay", false, ErrorMessage = "Date closure to must include a month")]
        [Range(1, 12, ErrorMessage = "Date of closure to month must be between {1} and {2}")]
        public int? DateToMonth { get; set; }

        [Display(Name = "Year")]
        [RequiredIf("IsSingleDay", false, ErrorMessage = "Date closure to must include a year")]
        [Range(2000, 2100, ErrorMessage = "Date of closure to year must be between {1} and {2}")]
        public int? DateToYear { get; set; }

        [Required(ErrorMessage = "Select the reason for the closure")]
        public int ReasonTypeId { get; set; }

        [Display(Name = "Other reason")]
        [MinLength(5, ErrorMessage = "Other reason must be 5 characters or more")]
        [MaxLength(1024, ErrorMessage = "Other reason must be 1024 characters or fewer")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Other, ErrorMessage = "Enter other reason")]
        [AlphaNumericLimitedSpecialChars]
        public string OtherReason { get; set; }

        [Display(Name = "How many cases have been recorded?")]
        [MinLength(5, ErrorMessage = "How many cases must be 5 characters or more")]
        [MaxLength(1024, ErrorMessage = "How many cases must be 1024 characters or fewer")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Covid, ErrorMessage = "Enter how many cases have been recorded")]
        [AlphaNumericLimitedSpecialChars]
        public string CovidQ1 { get; set; }

        [Display(Name = "Why did the whole school need to be closed?")]
        [MinLength(5, ErrorMessage = "Why the school closed must be 5 characters or more")]
        [MaxLength(1024, ErrorMessage = "Why the school closed must be 1024 characters or fewer")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Covid, ErrorMessage = "Enter why the shool needs to be closed")]
        [AlphaNumericLimitedSpecialChars]
        public string CovidQ2 { get; set; }

        [Display(Name = "Was an alternative considered in line with any contingency plans?")]
        [MinLength(5, ErrorMessage = "An alternative in line with contingency must be 5 characters or more")]
        [MaxLength(1024, ErrorMessage = "An alternative in line with contingency must be 1024 characters or fewer")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Covid, ErrorMessage = "Enter if an alternative was considered in line with an contingency plan")]
        [AlphaNumericLimitedSpecialChars]
        public string CovidQ3 { get; set; }

        [Display(Name = "Did PHA advise that the school needed to close for deep clean?")]
        [MinLength(5, ErrorMessage = "Did the PHA advise that the school needed to close for a deep clean must be 5 characters or more")]
        [MaxLength(1024, ErrorMessage = "Did the PHA advise that the school needed to close for a deep clean must be 1024 characters or fewer")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Covid, ErrorMessage = "Enter did PHA advise that the school needed to close for deep clean")]
        [AlphaNumericLimitedSpecialChars]
        public string CovidQ4 { get; set; }

        [Display(Name = "Provide details of the closure")]
        [MinLength(5, ErrorMessage = "Details of the closure must be 5 characters or more")]
        [MaxLength(1024, ErrorMessage = "Details of the closure must be 1024 characters or fewer")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Covid, ErrorMessage = "Enter details of the closure")]
        [AlphaNumericLimitedSpecialChars]
        public string CovidQ5 { get; set; }

        [Required(ErrorMessage = "Select if closure is for a single day or continous days")]
        public bool? IsSingleDay { get; set; }
        public List<ReasonTypeDto> ReasonTypeList { get; set; }
        public string InstitutionName { get; set; }
        public string Srn { get; set; }
    }
}