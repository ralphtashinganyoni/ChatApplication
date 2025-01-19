using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ChatApplication.Server.Domain.DTOs.Auth;
using ChatApplication.Server.Domain.DTOs.Common;
using ChatApplication.Server.Domain.Entities;

namespace ChatApplication.Server.Application.Features.Auth
{
    public class UserSignUpCommand : IRequest<Result<UserDto>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserSignUpCommandValidator : AbstractValidator<UserSignUpCommand>
    {
        public UserSignUpCommandValidator()
        {
            RuleFor(cmd => cmd.Username)
                .NotEmpty()
                .WithMessage("UserName is required");

            RuleFor(cmd => cmd.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email is required");


            RuleFor(cmd => cmd.Password)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A password is required");
        }
    }

    public class UserSignUpCommandHandler : IRequestHandler<UserSignUpCommand, Result<UserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserSignUpCommandHandler> _logger;

        public UserSignUpCommandHandler(
            UserManager<ApplicationUser> userManager,
            ILogger<UserSignUpCommandHandler> logger,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        public async Task<Result<UserDto>> Handle(UserSignUpCommand cmd, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting user creation for email: {Email}", cmd.Email);

            var user = new ApplicationUser
            {
                UserName = cmd.Username,
                Email = cmd.Email,
                // Will set this to true due to time constraints of email verification process
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                var passwordResult = await _userManager.AddPasswordAsync(user, cmd.Password);

                if (passwordResult.Succeeded)
                {
                    var userDto = new UserDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email
                    };

                    _logger.LogInformation("User creation successful for email: {Email}", cmd.Email);
                    return new SuccessResult<UserDto>(userDto);
                }
                else
                {
                    // Roll back the user creation
                    await _userManager.DeleteAsync(user);

                    var errorMessages = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    _logger.LogError("Failed to set password for user '{UserName}': {Errors}", cmd.Username, errorMessages);
                    return new FailureResult<UserDto>("Password setup failed: " + errorMessages);
                }
            }

            var createErrorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Failed to create user '{UserName}': {Errors}", cmd.Username, createErrorMessages);
            return new FailureResult<UserDto>("User creation failed: " + createErrorMessages);
        }

    }

}
