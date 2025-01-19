namespace ChatApplication.Server.Domain.DTOs.Auth
{
    public class SignUpRequestDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
