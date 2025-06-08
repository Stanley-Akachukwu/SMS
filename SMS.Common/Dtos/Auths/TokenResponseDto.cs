﻿

namespace SMS.Common.Dtos.Auths
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTimeOffset? TokenExpired { get; set; }
    }
}
