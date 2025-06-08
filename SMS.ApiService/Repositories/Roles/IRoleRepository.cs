
using SMS.Common.Dtos;
using SMS.Common.Dtos.Roles;

namespace SMS.ApiService.Repositories.Roles
{
    public interface IRoleRepository
    {
        Task<ApiResponse<string>> CreateOrUpdateRoleAsync(RoleDto role, CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<RoleDto>>?> GetRolesAsync(CancellationToken cancellationToken);
        Task<ApiResponse<IEnumerable<RolePermissionDto>>?> GetPermissionsByRoleIdAsync(string? id,CancellationToken cancellationToken);
        Task<ApiResponse<string>?> DeleteRole(string id, CancellationToken cancellationToken);
    }
}
