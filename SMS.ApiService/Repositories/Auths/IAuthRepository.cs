using SMS.ApiService.Entities.Users;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Auths;
using SMS.Common.Dtos.Users;

namespace SMS.ApiService.Repositories.Auths
{
    public interface IAuthRepository
    {
        Task<ApiResponse<User?>> RegisterAsync(UserDto request);
        Task<ApiResponse<TokenResponseDto?>> LoginAsync(UserDto request);
        Task<ApiResponse<TokenResponseDto?>> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
