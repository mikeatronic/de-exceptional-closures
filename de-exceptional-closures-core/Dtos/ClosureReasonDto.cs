using System;

namespace de_exceptional_closures_core.Dtos
{
    public class ClosureReasonDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string InstitutionName { get; set; }
        public string Srn { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? ReasonTypeId { get; set; }
        public string ReasonType { get; set; }
        public string OtherReason { get; set; }
        public bool? Approved { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? ApprovalTypeId { get; set; }
        public string ApprovalType { get; set; }
        public string RejectionReason { get; set; }
        public string OtherReasonCovid { get; set; }
    }
}