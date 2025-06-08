using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Entities.Roles;
using SMS.ApiService.Persistence;
using SMS.Common.Authorization;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Roles;
using SMS.Common.Enums;
using System.Data;

namespace SMS.ApiService.Repositories.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _dbContext;
        public RoleRepository(AppDbContext context)
        {
            _dbContext = context;
        }
        private ApiResponse<string> CopyFromExistingRole(string? copyFromRoleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private RolePermission MapToRolePermission(PermissionConfig permission, string roleId, string? createdBy) =>
            new()
            {
                IsActive = true,
                CreatedByUserId = createdBy,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                Description = permission.Description,
                Id = Ulid.NewUlid().ToString(),
                PermissionId = permission.Id,
                PermissionName = permission.Name,
                RoleId = roleId,
                ParentId = permission.ParentId.Value,
            };

        public async Task<ApiResponse<string>> CreateOrUpdateRoleAsync(RoleDto? role, CancellationToken cancellationToken)
        {
            var copyPermission = role.CopyPermissions;
            if(copyPermission != null)
                if (copyPermission.AllowedCopy == true && !string.IsNullOrEmpty(copyPermission?.CopyFromRoleId))
                    return CopyFromExistingRole(copyPermission?.CopyFromRoleId, cancellationToken);


            string action = "created";
            var rolePermissions = new List<RolePermission>();
            var parentPermissions = ParentPermissions.GetParentPermissions();

            try
            {
                var permissions = role.PermissionIds.Any()
                    ? await _dbContext.PermissionConfigs
                        .Where(p => role.PermissionIds.Contains(p.Id))
                        .ToListAsync(cancellationToken)
                    : new List<PermissionConfig>();

                permissions = GetAssignablePermissionsForRoleAsync(role.UserRoleType, permissions);


                var dbRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == role.Id, cancellationToken);



                if (dbRole == null)
                {
                    var newRole = new Role
                    {
                        Id = Ulid.NewUlid().ToString(),
                        Name = role.Name,
                        IsActive = true,
                        Description = role?.Description,
                        RoleType = role.UserRoleType.HasValue ? (int)role.UserRoleType.Value : 0,
                    };
                    _dbContext.Roles.Add(newRole);

                    rolePermissions.AddRange(permissions.Select(p => MapToRolePermission(p, newRole.Id,  role.CreatedById)));
                }
                else
                {
                    action = "updated";
                    dbRole.Name = role.Name;
                    dbRole.Description = role?.Description;
                    _dbContext.Roles.Update(dbRole);

                    var existingPermissions = await _dbContext.RolePermissions
                        .Where(p => p.RoleId == role.Id)
                        .ToListAsync(cancellationToken);

                    _dbContext.RolePermissions.RemoveRange(existingPermissions);

                    rolePermissions.AddRange(permissions.Select(p => MapToRolePermission(p, dbRole.Id, role.Name)));
                }

                if (rolePermissions.Any())
                    await _dbContext.RolePermissions.AddRangeAsync(rolePermissions, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);

                int statusCode = dbRole == null ? StatusCodes.Status201Created : StatusCodes.Status200OK;
                return ApiResponse<string>.Success($"Successfully {action}", statusCode);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }


        private List<PermissionConfig> GetAssignablePermissionsForRoleAsync(UserRoleType? userRoleType, List<PermissionConfig>? permissions)
        {
            if (userRoleType == null || userRoleType == UserRoleType.None || permissions == null)
                return permissions;

            IEnumerable<PermissionConfig> filteredPermissions = userRoleType switch
            {
                UserRoleType.Teacher or UserRoleType.Staff => permissions
                    .Where(p => p.StaffAssign == true || p.IsDefault ==true),

                UserRoleType.Parent => permissions
                    .Where(p => p.ParentAssign == true || p.IsDefault == true),

                UserRoleType.Admin => permissions
                    .Where(p => p.AdminAssign == true || p.IsDefault == true),

                _ => Enumerable.Empty<PermissionConfig>()
            };

            return filteredPermissions != null? filteredPermissions.ToList(): permissions;
        }


        public async Task<ApiResponse<IEnumerable<RoleDto>>> GetRolesAsync(CancellationToken cancellationToken)
        {
            try
            {
                
                //var allR = _dbContext.RolePermissions.Where(p=>p.RoleId!="System").ToList();
                //_dbContext.RolePermissions.RemoveRange(allR);
                //await _dbContext.SaveChangesAsync(cancellationToken);

                var roles = await _dbContext.Roles.ToListAsync(cancellationToken);
                var roleDtos = roles.Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    UserRoleType = role.RoleType> 0? (UserRoleType)role.RoleType: UserRoleType.None,
                }).ToList();

                return ApiResponse<IEnumerable<RoleDto>>.Success(roleDtos, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                var rsp = new ApiResponse<IEnumerable<RoleDto>>();
                rsp.ErrorMessage = ex.Message;
                return rsp;
            }
        }

        public async Task<ApiResponse<IEnumerable<RolePermissionDto>>?> GetPermissionsByRoleIdAsync(string? id, CancellationToken cancellationToken)
        {

            try
            {
                var systemPermissions = await _dbContext.RolePermissions
                    .Where(p => p.RoleId == "01JX3BYWMF1AWD900K93VEFC6A")
                    .ToListAsync(cancellationToken);

                var rolePermissions = await _dbContext.RolePermissions
                    .Where(p => p.RoleId == id)
                    .ToListAsync(cancellationToken);

                // Combine permissions
                var allPermissions = rolePermissions
                    .UnionBy(systemPermissions, p => p.PermissionId)
                    .ToList();

                var permissionDtos = allPermissions.Select(p => new RolePermissionDto
                {
                    Id = p.Id,
                    PermissionId = p.PermissionId,
                    PermissionName = p.PermissionName,
                    RoleId = p.RoleId,
                    ParentId = p.ParentId,
                    Assigned = p.RoleId == id,
                    Description = p.Description,
                }).ToList();
                return ApiResponse<IEnumerable<RolePermissionDto>>.Success(permissionDtos, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                var rsp = new ApiResponse<IEnumerable<RolePermissionDto>>();
                rsp.ErrorMessage = ex.Message;
                return rsp;
            }
        }

        public async Task<ApiResponse<string>> DeleteRole(string id, CancellationToken cancellationToken)
        {
            // Remove role permissions
            var rolePermissions = await _dbContext.RolePermissions
                .Where(p => p.RoleId == id)
                .ToListAsync(cancellationToken);

            _dbContext.RolePermissions.RemoveRange(rolePermissions);

            // Remove role
            var role = await _dbContext.Roles
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (role == null)
            {
                return new ApiResponse<string>
                {
                    ErrorMessage = "Role not found",
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = true
                };
            }

            _dbContext.Roles.Remove(role);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ApiResponse<string>
            {
                Data = $"Role with ID- {id} deleted successfully",
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true
            };
        }

    }
}
