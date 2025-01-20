namespace ChatApplication.Server.Infrastructure.Services
{
    using ChatApplication.Server.Domain.DTOs.Messages;
    using ChatApplication.Server.Domain.Entities;
    using ChatApplication.Server.Domain.Interfaces;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Service for managing chat functionality.
    /// </summary>
    public class ChatService : IChatService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpUserContextService _httpUserContextService;

        public ChatService(IMessageRepository messageRepository, IHttpUserContextService httpUserContextService)
        {
            _messageRepository = messageRepository;
            _httpUserContextService = httpUserContextService;
        }

        /// <inheritdoc />
        public async Task<List<MessageDto>> GetConversationAsync(string senderId, string receiverId)
        {
            var messages = await _messageRepository.GetConversationAsync(senderId, receiverId);

            return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.RecipientId,
                Content = m.Content,
                Timestamp = m.SentAt
            }).ToList();
        }

        /// <inheritdoc />
        public async Task<MessageDto> SaveMessageAsync(string senderId, string receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                RecipientId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            await _messageRepository.AddMessageAsync(message);

            return new MessageDto
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.RecipientId,
                Content = message.Content,
                Timestamp = message.SentAt
            };
        }
    }

}
