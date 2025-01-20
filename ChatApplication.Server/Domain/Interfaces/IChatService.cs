namespace ChatApplication.Server.Domain.Interfaces
{
    using ChatApplication.Server.Domain.DTOs.Messages;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Service for managing chat functionality.
    /// </summary>
    public interface IChatService
    {
        /// <summary>
        /// Retrieves the conversation between two users.
        /// </summary>
        /// <param name="senderId">The ID of the sender.</param>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <returns>A list of message DTOs representing the conversation.</returns>
        Task<List<MessageDto>> GetConversationAsync(string senderId, string receiverId);

        /// <summary>
        /// Saves a new message between two users.
        /// </summary>
        /// <param name="senderId">The ID of the sender.</param>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <param name="content">The content of the message.</param>
        /// <returns>A message DTO representing the saved message.</returns>
        Task<MessageDto> SaveMessageAsync(string senderId, string receiverId, string content);
    }
}
