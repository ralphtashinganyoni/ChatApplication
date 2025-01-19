namespace ChatApplication.Server.Domain.DTOs.Messages
{
    public class CreateMessageDto
    {
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
    }
}
