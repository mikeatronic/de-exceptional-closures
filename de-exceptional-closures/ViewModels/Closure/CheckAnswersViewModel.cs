using de_exceptional_closures_core.Dtos;
using System;
using System.Collections.Generic;

namespace de_exceptional_closures.ViewModels.Closure
{
    public class CheckAnswersViewModel : BaseViewModel
    {
        public CheckAnswersViewModel()
        {
            ReasonTypeList = new List<ReasonTypeDto>();
        }

        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public DateTime DateMultipleFrom { get; set; }
        public int? DateFromDay { get; set; }
        public int? DateFromMonth { get; set; }
        public int? DateFromYear { get; set; }
        public int? DateToDay { get; set; }
        public int? DateToMonth { get; set; }
        public int? DateToYear { get; set; }
        public int ReasonTypeId { get; set; }
        public string OtherReason { get; set; }
        public string OtherReasonCovid { get; set; }
        public string CovidQ1 { get; set; }
        public string CovidQ2 { get; set; }
        public string CovidQ3 { get; set; }
        public string CovidQ4 { get; set; }
        public string CovidQ5 { get; set; }
        public bool IsSingleDay { get; set; }
        public List<ReasonTypeDto> ReasonTypeList { get; set; }
        public int ApprovalTypeId { get; set; }
        public string InstitutionName { get; set; }
        public string ReasonType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}