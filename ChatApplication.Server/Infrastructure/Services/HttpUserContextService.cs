using ChatApplication.Server.Domain.Interfaces;
using System.Security.Claims;

namespace ChatApplication.Server.Infrastructure.Services
{
    public class HttpUserContextService : IHttpUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUserContextService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
        public HttpUserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves the user ID from the token in the current HTTP context.
        /// </summary>
        /// <returns>The user ID as a string, or null if not found.</returns>
        public string? GetUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return null;
            }

            // Retrieve the user ID from the "sub" claim or a custom claim, if applicable.
            return user.FindFirstValue(ClaimTypes.NameIdentifier) ??
                   user.FindFirstValue("sub");
        }
    }
}
