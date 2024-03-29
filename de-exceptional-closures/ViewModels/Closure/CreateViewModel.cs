﻿using de_exceptional_closures.Extensions;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Closure
{
    public class CreateViewModel : BaseViewModel
    {
        public CreateViewModel()
        {
            ReasonTypeList = new List<ReasonTypeDto>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Date from is required")]
        [Display(Name = "Date from")]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Date to")]
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; }

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
        public bool IsSingleDay { get; set; }
        public List<ReasonTypeDto> ReasonTypeList { get; set; }
        public string InstitutionName { get; set; }
    }
}
