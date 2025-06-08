using SMS.ApiService.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Roles
{
    public class Role : BaseEntity<string>
    {
        [MaxLength(128)]
        public string? Name { get; set; }
        public int? RoleType { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<RolePermission>? Permissions { get; set; }
    }
}
