namespace ChatApplication.Server.Infrastructure.Services
{
    using ChatApplication.Server.Domain.DTOs.Messages;
    using ChatApplication.Server.Domain.Entities;
    using ChatApplication.Server.Domain.Interfaces;
    using ChatApplication.Server.Persistence;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MessageDto>> GetConversationAsync(string senderId, string receiverId)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == senderId && m.RecipientId == receiverId) ||
                            (m.SenderId == receiverId && m.RecipientId == senderId))
                .OrderBy(m => m.SentAt)
                .Select(m => new MessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.RecipientId,
                    Content = m.Content,
                    Timestamp = m.SentAt
                })
                .ToListAsync();
        }

        public async Task<MessageDto> SaveMessageAsync(string senderId, string receiverId, string content)
        {
            var message = new Message
            {
                SenderId = senderId,
                RecipientId = receiverId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

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
