
using SMS.ApiService.Entities.Roles;
using SMS.Common.Enums;

namespace SMS.ApiService.Persistence.DataSeed
{
    public static class PermissionSeed 
    {
        private static readonly Ulid SystemUserId = Ulid.Empty;
        public static async Task<PermissionConfig[]> GetPermissionConfigsAsync(CancellationToken cancellationToken)
        {
            return new PermissionConfig[]
            {
                 new PermissionConfig
                {
                    Id = (int)Permission.CanViewAllDashboard,
                    ParentId = (int)ParentPermission.General,
                    Name = Permission.CanViewAllDashboard.ToString(),
                    Description = "Can View All Dashboards",
                    IsActive = true,
                    CreatedByUserId = SystemUserId.ToString(),
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UpdatedByUserId = SystemUserId.ToString(),
                    IsDefault = true,
                    AdminAssign = true,
                    ParentAssign = true,
                    StaffAssign = true,
                },
                  new PermissionConfig
                {
                    Id = (int)Permission.CanDeletePermisions,
                     ParentId = (int)ParentPermission.Administration,
                    Name = Permission.CanDeletePermisions.ToString(),
                    Description = "Can Delete Permission",
                    IsActive = true,
                    CreatedByUserId = SystemUserId.ToString(),
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UpdatedByUserId = SystemUserId.ToString(),
                    IsDefault = false,
                    AdminAssign = true,
                    ParentAssign = false,
                    StaffAssign = false,
                },
                   new PermissionConfig
                {
                    Id = (int)Permission.CanViewPermisions,
                     ParentId = (int)ParentPermission.Administration,
                    Name = Permission.CanViewPermisions.ToString(),
                    Description = "Can View Permissions",
                    IsActive = true,
                    CreatedByUserId = SystemUserId.ToString(),
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UpdatedByUserId = SystemUserId.ToString(),
                    IsDefault = true,
                    AdminAssign = true,
                    ParentAssign = false,
                    StaffAssign = false,
                },
                    new PermissionConfig
                {
                    Id = (int)Permission.CanUpdatePermisions,
                     ParentId = (int)ParentPermission.Administration,
                    Name = Permission.CanUpdatePermisions.ToString(),
                    Description = "Can Update Permissions",
                    IsActive = true,
                    CreatedByUserId = SystemUserId.ToString(),
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UpdatedByUserId = SystemUserId.ToString(),
                    IsDefault = false,
                    AdminAssign = true,
                    ParentAssign = false,
                    StaffAssign = false,
                },
           };
        }
    }
}
