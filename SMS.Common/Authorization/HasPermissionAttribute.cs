using Microsoft.AspNetCore.Authorization;
using SMS.Common.Enums;

namespace SMS.Common.Authorization
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission)
            : base(policy: permission.ToString())
        {
        }
    }
}
