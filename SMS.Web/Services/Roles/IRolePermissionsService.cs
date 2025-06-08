
using SMS.Common.Dtos;
using SMS.Common.Dtos.Roles;

namespace SMS.Web.Services.Roles
{
    public interface IRolePermissionsService
    {
        Task<ApiResponse<string>> CreateOrUpdateRoleAsync(RoleDto role, CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<RoleDto>>?> GetRolesAsync(CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<RolePermissionDto>>?> GetRolePermissionsAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<string>> DeleteRoleAsync(string id, CancellationToken cancellationToken);
    }
}
