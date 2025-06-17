using SMS.ApiService.Entities.ClassUnits;
using SMS.ApiService.Entities.Schools;
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

        public ICollection<ClassUnit> Groups { get; set; } = new List<ClassUnit>();
    }
}


