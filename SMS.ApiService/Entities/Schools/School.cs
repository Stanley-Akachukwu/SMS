using SMS.ApiService.Entities.SchoolClasses;
using SMS.ApiService.Entities.Settings;
using SMS.Common.Dtos.Schools;
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Schools
{
    public class School : BaseEntity<string>
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        public string? SettingId { get; set; }
        public SchoolSetting Setting { get; set; } = new();
        public ICollection<SchoolClass> Classes { get; set; } = new List<SchoolClass>();
    }
}

