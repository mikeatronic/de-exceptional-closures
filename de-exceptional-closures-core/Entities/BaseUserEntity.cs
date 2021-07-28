namespace de_exceptional_closures_core.Entities
{
    public class BaseUserEntity<T> : BaseEntity<T>
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
    }
}