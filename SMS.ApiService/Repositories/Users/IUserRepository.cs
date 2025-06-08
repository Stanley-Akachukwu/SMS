using SMS.Common.Dtos.Departments;
using SMS.Common.Dtos.Users;
using SMS.Common.Dtos;

namespace SMS.ApiService.Repositories.Users
{
    public interface IUserRepository
    {
        Task<ApiResponse<UserDto>> GetUserInfoByEmailAsync(string email);
        Task<ApiResponse<string>> CreateOrUpdateUserAsync(UserDto user, CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<UserDto>>> GetUsersAsync(CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<DepartmentDto>>?> GetDepartmentsAsync(CancellationToken cancellationToken);
    }
}
