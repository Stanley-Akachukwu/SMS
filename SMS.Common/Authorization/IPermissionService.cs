using SMS.Common.Enums;

namespace SMS.Common.Authorization
{
    public interface IPermissionService
    {
        Task<List<Permission>> GetPermissionsAsync(string? userId);
        Task<bool> HasPermissionAsync(Permission permission, string? userId);
    }
}
