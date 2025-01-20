using ChatApplication.Server.Domain.Entities;

namespace ChatApplication.Server.Domain.Interfaces
{
    /// <summary>
    /// Defines the contract for managing Message entities.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Retrieves a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message.</param>
        /// <returns>The message if found; otherwise, null.</returns>
        Task<Message> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all messages.
        /// </summary>
        /// <returns>A collection of all messages.</returns>
        Task<IEnumerable<Message>> GetAllAsync();

        /// <summary>
        /// Retrieves all messages sent by a specific sender.
        /// </summary>
        /// <param name="senderId">The unique identifier of the sender.</param>
        /// <returns>A collection of messages sent by the specified sender.</returns>
        Task<IEnumerable<Message>> GetBySenderIdAsync(string senderId);

        /// <summary>
        /// Retrieves all messages received by a specific recipient.
        /// </summary>
        /// <param name="recipientId">The unique identifier of the recipient.</param>
        /// <returns>A collection of messages received by the specified recipient.</returns>
        Task<IEnumerable<Message>> GetByRecipientIdAsync(string recipientId);

        /// <summary>
        /// Adds a new message to the repository.
        /// </summary>
        /// <param name="message">The message to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Message message);

        /// <summary>
        /// Updates an existing message in the repository.
        /// </summary>
        /// <param name="message">The message to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Message message);

        /// <summary>
        /// Deletes a message by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the message to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Marks a message as read.
        /// </summary>
        /// <param name="id">The unique identifier of the message to mark as read.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task MarkAsReadAsync(Guid id);

        /// <summary>
        /// Retrieves paged messages for a specific user (as sender or recipient).
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="pageNumber">The current page number (starting from 1).</param>
        /// <param name="pageSize">The number of messages to include per page.</param>
        /// <returns>A paged collection of messages for the specified user.</returns>
        Task<(IEnumerable<Message> Messages, int TotalCount)> GetPagedMessagesByUserIdAsync(string userId, int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves the conversation between two users.
        /// </summary>
        /// <param name="senderId">The ID of the sender.</param>
        /// <param name="receiverId">The ID of the receiver.</param>
        /// <returns>A list of messages between the two users.</returns>
        Task<List<Message>> GetConversationAsync(string senderId, string receiverId);

        /// <summary>
        /// Adds a new message to the database.
        /// </summary>
        /// <param name="message">The message entity to add.</param>
        Task AddMessageAsync(Message message);

    }

}
