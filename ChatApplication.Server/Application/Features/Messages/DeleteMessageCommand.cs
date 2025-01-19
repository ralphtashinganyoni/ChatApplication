using ChatApplication.Server.Domain.DTOs.Common;
using ChatApplication.Server.Domain.Entities;
using ChatApplication.Server.Domain.Interfaces;
using ChatApplication.Server.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApplication.Server.Application.Features.Messages
{
    public class DeleteMessageCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteMessageCommandValidator : AbstractValidator<DeleteMessageCommand>
    {
        public DeleteMessageCommandValidator()
        {
            RuleFor(cmd => cmd.Id)
                .NotEmpty()
                .WithMessage("Username is required");
        }
    }

    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result<string>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpUserContextService _userContextService;
        private readonly ILogger<DeleteMessageCommandHandler> _logger;

        public DeleteMessageCommandHandler(
            IMessageRepository messageRepository,
            IHttpUserContextService userContextService,
            ILogger<DeleteMessageCommandHandler> logger)
        {
            _messageRepository = messageRepository;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(DeleteMessageCommand cmd, CancellationToken cancellationToken)
        {
            // Get the user ID from the current HTTP context
            var userId = _userContextService.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthenticated user attempted to delete a message.");
                return new FailureResult<string>("User is not authenticated.");
            }

            _logger.LogInformation("User {UserId} is attempting to delete message {MessageId}.", userId, cmd.Id);

            // Retrieve the message
            var message = await _messageRepository.GetByIdAsync(cmd.Id);
            if (message == null)
            {
                _logger.LogWarning("Message {MessageId} not found for user {UserId}.", cmd.Id, userId);
                return new FailureResult<string>("Message not found.");
            }

            // Check if the user is the sender or recipient
            if (message.SenderId != userId && message.RecipientId != userId)
            {
                _logger.LogWarning("User {UserId} does not have permission to delete message {MessageId}.", userId, cmd.Id);
                return new FailureResult<string>("You do not have permission to delete this message.");
            }

            // Perform the soft delete based on who is deleting
            if (message.SenderId == userId)
            {
                message.IsDeletedBySender = true;
            }
            else if (message.RecipientId == userId)
            {
                message.IsDeletedByRecipient = true;
            }

            message.DeletedAt = DateTime.UtcNow;
            message.DeletedBy = userId;

            await _messageRepository.UpdateAsync(message);

            _logger.LogInformation("Message {MessageId} was successfully deleted by user {UserId}.", cmd.Id, userId);
            return new SuccessResult<string>($"Message {cmd.Id} deleted successfully.");
        }
    }

}
