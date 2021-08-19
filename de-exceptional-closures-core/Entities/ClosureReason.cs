using System;

namespace de_exceptional_closures_core.Entities
{
    public class ClosureReason : BaseUserEntity<int>
    {
        public string InstitutionName { get; set; }
        public string Srn { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? ReasonTypeId { get; set; }
        public ReasonType ReasonType { get; set; }
        public string OtherReason { get; set; }
        public bool? Approved { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? ApprovalTypeId { get; set; }
        public ApprovalType ApprovalType { get; set; }
        public int? RejectionReasonId { get; set; }
        public RejectionReason RejectionReason { get; set; }
        public string OtherReasonCovid { get; set; }
        public string CovidQ1 { get; set; }
        public string CovidQ2 { get; set; }
        public string CovidQ3 { get; set; }
        public string CovidQ4 { get; set; }
        public string CovidQ5 { get; set; }
    }
}