using ChatApplication.Server.Domain.DTOs.Common;
using ChatApplication.Server.Domain.Entities;
using ChatApplication.Server.Domain.Interfaces;
using ChatApplication.Server.Infrastructure.Services;
using MediatR;

namespace ChatApplication.Server.Application.Features.Messages
{
    public class GetPagedUserMessagesQuery : IRequest<Result<PagedList<Message>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetPagedUserMessagesQueryHandler : IRequestHandler<GetPagedUserMessagesQuery, Result<PagedList<Message>>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpUserContextService _userContextService;
        private readonly ILogger<GetPagedUserMessagesQueryHandler> _logger;

        public GetPagedUserMessagesQueryHandler(
            IMessageRepository messageRepository,
            IHttpUserContextService userContextService,
            ILogger<GetPagedUserMessagesQueryHandler> logger)
        {
            _messageRepository = messageRepository;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<Result<PagedList<Message>>> Handle(GetPagedUserMessagesQuery qry, CancellationToken cancellationToken)
        {
            // Get the user ID from the context
            var userId = _userContextService.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthenticated user attempted to retrieve messages.");
                return new FailureResult<PagedList<Message>>("User is not authenticated.");
            }

            _logger.LogInformation("Fetching paged messages for user {UserId}, Page: {PageNumber}, Size: {PageSize}",
                                   userId, qry.PageNumber, qry.PageSize);

            // Retrieve paged messages for the user
            var pagedMessages = await _messageRepository.GetPagedMessagesByUserIdAsync(userId, qry.PageNumber, qry.PageSize);

            if (pagedMessages.TotalCount == 0)
            {
                _logger.LogInformation("No messages found for user {UserId}.", userId);
                return new SuccessResult<PagedList<Message>>(new PagedList<Message>());
            }

            _logger.LogInformation("Retrieved {MessageCount} messages for user {UserId}.", pagedMessages.TotalCount, userId);
            return new SuccessResult<PagedList<Message>>(new PagedList<Message>
            {
                Items = pagedMessages.Messages,
                TotalCount = pagedMessages.TotalCount,
                PageNumber = qry.PageNumber,
                PageSize = qry.PageSize
            });
        }
    }
}