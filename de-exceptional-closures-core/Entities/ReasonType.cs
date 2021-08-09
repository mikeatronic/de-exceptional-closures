namespace de_exceptional_closures_core.Entities
{
    public class ReasonType : BaseEntity<int>
    {
        public string Description { get; set; }
        public bool? ApprovalRequired { get; set; }
    }
}