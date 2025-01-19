namespace ChatApplication.Server.Domain.Interfaces
{
    using ChatApplication.Server.Domain.DTOs.Messages;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IChatService
    {
        Task<List<MessageDto>> GetConversationAsync(string senderId, string receiverId);
        Task<MessageDto> SaveMessageAsync(string senderId, string receiverId, string content);
    }
}
