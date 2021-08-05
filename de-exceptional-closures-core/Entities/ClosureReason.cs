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
    }
}