using ChatApplication.Server.Application.Features.Auth;
using ChatApplication.Server.Domain.DTOs.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChatApplication.Server.Application.Extensions;

namespace ChatApplication.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(ILogger<AuthController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Authenticates a user and generates an access token for valid credentials.
        /// </summary>
        /// <param name="request">The login request containing the username and password.</param>
        /// <returns>
        /// A result containing the access token if authentication is successful.
        /// Returns <see cref="StatusCodes.Status200OK"/> if successful,
        /// <see cref="StatusCodes.Status400BadRequest"/> if the input is invalid,
        /// </returns>
        [HttpPost("sign-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDto request)
        {
            var command = new UserSignInCommand
            {
                Username = request.Username,
                Password = request.Password,
            };
            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }

        /// <summary>
        /// Registers a new user with the provided details.
        /// </summary>
        /// <param name="request">The registration request containing username and email.</param>
        /// <returns>
        /// A result indicating the success of the user creation process.
        /// Returns <see cref="StatusCodes.Status200OK"/> if successful,
        /// <see cref="StatusCodes.Status400BadRequest"/> if the input is invalid,
        /// </returns>
        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto request)
        {
            var command = new UserSignUpCommand
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password
            };

            var result = await _mediator.Send(command);
            return result.ToActionResult();
        }
    }
}
