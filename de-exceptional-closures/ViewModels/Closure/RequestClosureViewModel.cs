using de_exceptional_closures.Extensions;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Closure
{
    public class RequestClosureViewModel : BaseViewModel
    {
        public RequestClosureViewModel()
        {
            ReasonTypeList = new List<ReasonTypeDto>();
        }

        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }


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