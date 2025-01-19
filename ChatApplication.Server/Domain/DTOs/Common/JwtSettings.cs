namespace ChatApplication.Server.Domain.DTOs.Common
{
    public class JwtSettings
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string Secret { get; set; }
        public required string FrontendDomain { get; set; }
        public int ExpiryHours { get; set; } = 120;
        public int RefreshTokenLength { get; set; } = 300;
        public int VerifyEmailTokenLength { get; set; } = 60;
        public int ResetTokenLength { get; set; } = 60;
        public int ResetTokenExpiryInMinutes { get; set; } = 5;
    }
}
