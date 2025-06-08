
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Roles
{
    public class RolePermission : BaseEntity<string>
    {
        [MaxLength(128)]
        public int PermissionId { get; set; }
        [MaxLength(128)]
        public string? PermissionName { get; set; }
        public int ParentId { get; set; }
        [MaxLength(128)]
        public string? RoleId { get; set; }
        public Role? AppRole { get; set; }
    }
}
