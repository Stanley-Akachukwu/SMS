
using SMS.Common.Enums;

namespace SMS.Common.Dtos.Roles
{
    public class RoleDto 
    {
        public string? Id { get; set; } = string.Empty;
        public string? Name { get; set; }
        public UserRoleType? UserRoleType { get; set; }
        public SMSCoreAction Action { get; set; } = SMSCoreAction.None;
        public string? CreatedById { get; set; }
        public CopyPermissionDto? CopyPermissions { get; set; }
        public List<int>? PermissionIds { get; set; } = new List<int>();
        public List<RolePermissionDto>? RolePermissions { get; set; } = new();
        public List<RoleDto>? RoleDtos { get; set; } = new();
        public string? Description { get; set; } = string.Empty;
    }
    public class CopyPermissionDto
    {
        public bool? AllowedCopy { get; set; }
        public string? CopyFromRoleId { get; set; } = string.Empty;
    }
}
