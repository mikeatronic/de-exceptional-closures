using de_exceptional_closures.Extensions;
using de_exceptional_closures_core.Common;
using de_exceptional_closures_core.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace de_exceptional_closures.ViewModels.Closure
{
    public class EditReasonViewModel : BaseViewModel
    {
        public EditReasonViewModel()
        {
            ReasonTypeList = new List<ReasonTypeDto>();
        }

        public int Id { get; set; }
        public int ApprovalTypeId { get; set; }

        [Required(ErrorMessage = "Reason type required")]
        public int ReasonTypeId { get; set; }

        [Display(Name = "Other reason")]
        [MinLength(5, ErrorMessage = "Reason must be at least 5 characters long")]
        [MaxLength(1024, ErrorMessage = "Only 1024 characters are allowed")]
        [RequiredIf("ReasonTypeId", (int)OtherReasonType.Other, ErrorMessage = "Please enter other reason")]
        [AlphaNumericLimitedSpecialChars]
        public string OtherReason { get; set; }
        public List<ReasonTypeDto> ReasonTypeList { get; set; }
        public string InstitutionName { get; set; }
    }
}