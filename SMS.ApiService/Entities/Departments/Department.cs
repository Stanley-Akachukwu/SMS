using SMS.ApiService.Entities.Users;
using SMS.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Departments
{
    public class Department : BaseEntity<int>
    {
        [MaxLength(128)]
        public string? Name { get; set; }
        public MenuSection MenuSectionMap { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
