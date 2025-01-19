using ChatApplication.Server.Application.Extensions;
using ChatApplication.Server.Application.Features.Messages;
using ChatApplication.Server.Domain.DTOs.Common;
using ChatApplication.Server.Domain.DTOs.Messages;
using ChatApplication.Server.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMediator mediator, ILogger<MessagesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new message from the logged-in user to a specified recipient.
        /// </summary>
        /// <param name="createMessageDto">The details of the message to be created.</param>
        /// <returns>
        /// The created message as a <see cref="MessageDto"/> if successful.
        /// Returns <see cref="BadRequestObjectResult"/> if the message creation fails.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto request)
        {
            var command = new CreateMessageCommand
            {
                Content = request.Content,
                RecipientId = request.RecipientId,
                SenderId = request.SenderId,
            };

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// Retrieves paginated messages for the currently logged-in user.
        /// </summary>
        /// <param name="messageParams">Parameters for filtering and pagination.</param>
        /// <returns>
        /// A paginated list of messages as <see cref="PagedList{MessageDto}"/>.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessagesForUser([FromQuery] MessageParamsDto request)
        {
            var query = new GetPagedUserMessagesQuery
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            };

            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }

        /// <summary>
        /// Deletes a message with the specified ID for the logged-in user.
        /// </summary>
        /// <param name="id">The ID of the message to be deleted.</param>
        /// <returns>
        /// Returns <see cref="OkResult"/> if the message is deleted successfully.
        /// Returns <see cref="UnauthorizedResult"/> if the user is not authorized to delete the message.
        /// </returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteMessage(Guid id)
        {
            var command = new DeleteMessageCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }

}
