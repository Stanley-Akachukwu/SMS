using Microsoft.AspNetCore.Authorization;
using SMS.Common.Enums;

namespace SMS.Common.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public Permission Permission { get; }

        public PermissionRequirement(Permission permission)
        {
            Permission = permission;
        }
    }
}
