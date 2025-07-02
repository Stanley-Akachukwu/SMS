using SMS.ApiService.Entities.ClassUnits;
using SMS.ApiService.Entities.Schools;
using SMS.ApiService.Entities.Settings;
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.SchoolClasses
{
    public class SchoolClass:BaseEntity<string>
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        public string? SchoolId { get; set; }
        public School? School { get; set; }
        public string? SettingId { get; set; }
        public ClassSetting Setting { get; set; } = new();
        public ICollection<ClassUnit> Groups { get; set; } = new List<ClassUnit>();
    }
}


