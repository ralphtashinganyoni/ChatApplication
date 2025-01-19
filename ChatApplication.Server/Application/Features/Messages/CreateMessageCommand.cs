using ChatApplication.Server.Domain.DTOs.Common;
using ChatApplication.Server.Domain.Entities;
using ChatApplication.Server.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace ChatApplication.Server.Application.Features.Messages
{
    public class CreateMessageCommand : IRequest<Result<string>>
    {
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
        public string Content { get; set; }
    }


    public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
    {
        public CreateMessageCommandValidator()
        {
            RuleFor(cmd => cmd.SenderId)
                .NotEmpty()
                .WithMessage("Sender ID is required.");

            RuleFor(cmd => cmd.RecipientId)
                .NotEmpty()
                .WithMessage("Recipient ID is required.");

            RuleFor(cmd => cmd.Content)
                .NotEmpty()
                .WithMessage("Message content cannot be empty.")
                .MaximumLength(500)
                .WithMessage("Message content cannot exceed 500 characters.");
        }
    }


    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, Result<string>>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly ILogger<CreateMessageCommandHandler> _logger;

        public CreateMessageCommandHandler(
            IMessageRepository messageRepository,
            ILogger<CreateMessageCommandHandler> logger)
        {
            _messageRepository = messageRepository;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(CreateMessageCommand cmd, CancellationToken cancellationToken)
        {
            // Log the attempt to create a message
            _logger.LogInformation("Creating message from {SenderId} to {RecipientId}.", cmd.SenderId, cmd.RecipientId);

            // Create the message entity
            var message = new Message
            {
                Id = Guid.NewGuid(),
                SenderId = cmd.SenderId,
                RecipientId = cmd.RecipientId,
                Content = cmd.Content,
                SentAt = DateTime.UtcNow
            };

            try
            {
                // Save the message using the repository
                await _messageRepository.AddAsync(message);
                _logger.LogInformation("Message created successfully with ID: {MessageId}.", message.Id);

                return new SuccessResult<string>(message.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating the message.");
                throw;
            }
        }
    }

}
