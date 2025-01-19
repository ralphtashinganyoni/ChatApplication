using ChatApplication.Server.Application.Features.Auth;
using ChatApplication.Server.Application.Features.Users;
using ChatApplication.Server.Domain.DTOs.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChatApplication.Server.Application.Extensions;


namespace ChatApplication.Server.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public UserController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of all registered users in the system.
        /// </summary>
        /// <returns>
        /// A result containing a list of users if the operation is successful.
        /// Returns:
        /// <list type="bullet">
        /// <item><description><see cref="StatusCodes.Status200OK"/> with a list of users on success.</description></item>
        /// <item><description><see cref="StatusCodes.Status400BadRequest"/> if the request fails due to an error.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Returns the list of users.</response>
        /// <response code="400">Returns an error message if the operation fails.</response>
        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return result.ToActionResult();
        }
    }
}
