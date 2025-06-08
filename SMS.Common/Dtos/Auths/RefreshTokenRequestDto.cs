
namespace SMS.Common.Dtos.Auths
{
    public class RefreshTokenRequestDto
    {
        public string? UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
