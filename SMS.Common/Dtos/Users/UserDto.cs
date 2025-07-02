using SMS.Common.Dtos.Departments;
using SMS.Common.Dtos.Roles;
using SMS.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SMS.Common.Dtos.Users
{
    public class UserDto
    {
        public string? Id { get; set; } = string.Empty;
        [Display(Name = "User Name/Email")]
        public string? UserName { get; set; } = string.Empty;
        public string? RoleId { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }  
        public string? Password { get; set; } = string.Empty;
        [Display(Name = "First Name")]
        public string? FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; } = string.Empty;
        public string? SurName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Gender { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string? CreatedByUserId { get; set; } = string.Empty;
        public RoleDto? Role { get; set; }
        public DepartmentDto? Department { get; set; }
        public SMSCoreAction Action { get; set; }
        public List<RoleDto>? Roles { get; set; } = new();
        public List<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
         
    }
}
