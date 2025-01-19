namespace ChatApplication.Server.Domain.Entities
{
    public class BaseEntity
    {
        public string CreatedBy { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; } = string.Empty;
        public DateTimeOffset ModifiedAt { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
        public DateTimeOffset DeletedAt { get; set; }
    }
}
