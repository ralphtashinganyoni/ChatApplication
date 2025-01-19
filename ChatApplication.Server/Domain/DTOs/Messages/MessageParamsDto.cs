namespace ChatApplication.Server.Domain.DTOs.Messages
{
    public class MessageParamsDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
