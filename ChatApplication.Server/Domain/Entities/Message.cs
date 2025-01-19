using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApplication.Server.Domain.Entities
{
    public class Message : BaseEntity
    {
        public Guid Id { get; set; }
        // Foreign Key for Sender
        public string SenderId { get; set; }
        [ForeignKey("SenderId")]
        public ApplicationUser Sender { get; set; }

        // Foreign Key for Recipient
        public string RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public ApplicationUser Recipient { get; set; }

        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public bool IsDeletedBySender { get; set; } = false;
        public bool IsDeletedByRecipient { get; set; } = false;
    }
}
