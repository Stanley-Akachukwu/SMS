using SMS.Common.Dtos.Departments;
using SMS.Common.Dtos.Roles;
using SMS.Common.Enums;

namespace SMS.Common.Dtos.Users
{
    public class UserProfileDto
    {
        public string? IdentityUserId { get; set; }
        public string? Role { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Department { get; set; }
        public int? DepartmentId { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? IdentityRoleId { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public Ulid CreatedByUserId { get; set; }
        public SMSCoreAction Action { get; set; }
        public List<RoleDto>? Roles { get; set; } = new();
        public List<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();
    }
}
