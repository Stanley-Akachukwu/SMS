
using SMS.Common.Dtos;
using SMS.Common.Dtos.Departments;
using SMS.Common.Dtos.Users;

namespace SMS.Web.Services.Users
{
    public interface IUsersService
    {
        //Task<ApiResponse<UserLoginResponseDto>> CreateUserAsync(string email);
        Task<ApiResponse<string>> CreateOrUpdateUserAsync(UserDto userSetup, CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<UserDto>>?> GetUsersAsync(CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteRoleAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<DepartmentDto>>?> GetDepartmentsAsync(CancellationToken cancellationToken);
    }
}
