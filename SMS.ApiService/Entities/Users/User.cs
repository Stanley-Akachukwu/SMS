using SMS.ApiService.Entities.Departments;
using SMS.ApiService.Entities.Roles;
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Users
{
    public class User:BaseEntity<string>
    {
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        
        public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(150)]
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        [MaxLength(128)]
        public string? RoleId { get; set; }
        public Role? Role { get; set; }
        //[MaxLength(128)]
        //public string? UserProfileId { get; set; }
        //public UserProfile UserProfile { get; set; } = new UserProfile();

        [MaxLength(150)]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        [MaxLength(128)]
        public string? FirstName { get; set; }
        [MaxLength(128)]
        public string? MiddleName { get; set; }
        [MaxLength(128)]
        public string? SurName { get; set; }
    }
}
