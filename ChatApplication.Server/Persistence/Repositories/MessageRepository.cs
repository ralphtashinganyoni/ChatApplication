using ChatApplication.Server.Domain.Entities;
using ChatApplication.Server.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Server.Persistence.Repositories
{
    /// <summary>
    /// Implements the IMessageRepository interface for managing Message entities.
    /// </summary>
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to use.</param>
        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Message> GetByIdAsync(Guid id)
        {
            return await _context.Set<Message>()
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Message>> GetAllAsync()
        {
            return await _context.Set<Message>()
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Message>> GetBySenderIdAsync(string senderId)
        {
            return await _context.Set<Message>()
                .Where(m => m.SenderId == senderId && !m.IsDeletedBySender)
                .Include(m => m.Recipient)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Message>> GetByRecipientIdAsync(string recipientId)
        {
            return await _context.Set<Message>()
                .Where(m => m.RecipientId == recipientId && !m.IsDeletedByRecipient)
                .Include(m => m.Sender)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task AddAsync(Message message)
        {
            await _context.Set<Message>().AddAsync(message);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Message message)
        {
            _context.Set<Message>().Update(message);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id)
        {
            var message = await GetByIdAsync(id);
            if (message != null)
            {
                _context.Set<Message>().Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task MarkAsReadAsync(Guid id)
        {
            var message = await GetByIdAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await UpdateAsync(message);
            }
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Message> Messages, int TotalCount)> GetPagedMessagesByUserIdAsync(string userId, int pageNumber, int pageSize)
        {
            var query = _context.Set<Message>()
                .Where(m => (m.SenderId == userId && !m.IsDeletedBySender) ||
                            (m.RecipientId == userId && !m.IsDeletedByRecipient))
                .Include(m => m.Sender)
                .Include(m => m.Recipient);

            // Total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var messages = await query
                .OrderByDescending(m => m.SentAt) // Adjust ordering as needed
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (messages, totalCount);
        }

    }

}
