using SMS.Common.Dtos.Roles;
using SMS.Common.Enums;

namespace SMS.Common.Authorization
{
    public static class ParentPermissions
    {
        public static List<ParentPermissionDto> GetParentPermissions()
        {
            return Enum.GetValues(typeof(ParentPermission))
                .Cast<ParentPermission>()
                .Where(p => p != ParentPermission.None)  
                .Select(p => new ParentPermissionDto
                {
                    Id = p.ToString()+(int)p,
                   Name = p.ToString(),
                   Description = p.ToString(),
                   ParentId = (int)p
                })
                .ToList();
        }
    }
}
