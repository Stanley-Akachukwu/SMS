 
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Users
{
    public class UserProfile : BaseEntity<string>
    {
        [MaxLength(150)]
        public string? Email { get; set; }
        [MaxLength(65)]
        public string? FirstName { get; set; }
        [MaxLength(65)]
        public string? Surname { get; set; }
        [MaxLength(6)]
        public string? Gender { get; set; }
        [MaxLength(32)]
        public string? PhoneNumber { get; set; }
        
    }
}
