
using ChatApplication.Server.Domain.Entities;

namespace ChatApplication.Server.Domain.DTOs.Auth
{
    public class SignInResponseDto
    {
        public ApplicationUser ApplicationUser { get; set; }
        public string Token { get; set; }
    }
}
