using ChatApplication.Server.Domain.DTOs.Auth;
using ChatApplication.Server.Domain.DTOs.Common;
using ChatApplication.Server.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApplication.Server.Application.Features.Auth
{
    public class UserSignInCommand : IRequest<Result<SignInResponseDto>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SignInCommandValidator : AbstractValidator<UserSignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(cmd => cmd.Username)
                .NotEmpty()
                .WithMessage("Username is required");

            RuleFor(cmd => cmd.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }

    public class SignInCommandHandler : IRequestHandler<UserSignInCommand, Result<SignInResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SignInCommandHandler> _logger;

        public SignInCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger<SignInCommandHandler> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Result<SignInResponseDto>> Handle(UserSignInCommand cmd, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting sign-in for user: {Username}", cmd.Username);

            var user = await _userManager.FindByNameAsync(cmd.Username);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", cmd.Username);
                return new FailureResult<SignInResponseDto>("Invalid username or password.");
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, cmd.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
            {
                _logger.LogWarning("Invalid credentials for user: {Username}", cmd.Username);
                return new FailureResult<SignInResponseDto>("Invalid username or password.");
            }

            var token = GenerateJwtToken(user);
            _logger.LogInformation("User signed in successfully: {Username}", cmd.Username);

            return new SuccessResult<SignInResponseDto>(new SignInResponseDto
            {
                ApplicationUser = user,
                Token = token,
            });
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var roles = _userManager.GetRolesAsync(user).Result;
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpiryMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
