using ChatApplication.Server.Domain.DTOs.Common;
using ChatApplication.Server.Domain.Entities;
using ChatApplication.Server.Domain.Interfaces;
using global::ChatApplication.Server.Domain.DTOs.Auth;
using global::MediatR;
using Microsoft.AspNetCore.Identity;
namespace ChatApplication.Server.Application.Features.Users
{
    public class GetAllUsersQuery : IRequest<Result<List<UserDto>>>
    {
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;
        private readonly IHttpUserContextService _httpUserContextService;

        public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager, ILogger<GetAllUsersQueryHandler> logger, IHttpUserContextService httpUserContextService)
        {
            _userManager = userManager;
            _logger = logger;
            _httpUserContextService = httpUserContextService;
        }

        public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all users.");

            try
            {
                var loggedInUserId = _httpUserContextService.GetUserId();
                var users = _userManager.Users.Where(a=>a.Id != loggedInUserId)
                    .Select(user => new UserDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email
                    })
                    .ToList();

                _logger.LogInformation("Successfully fetched {Count} users.", users.Count);

                return new SuccessResult<List<UserDto>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
                return new FailureResult<List<UserDto>>("An error occurred while fetching users.");
            }
        }

    }
}
