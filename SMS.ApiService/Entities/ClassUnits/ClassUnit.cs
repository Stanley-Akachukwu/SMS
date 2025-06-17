using SMS.ApiService.Entities.SchoolClasses;
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.ClassUnits
{
    public class ClassUnit : BaseEntity<string>
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public bool IsClassPublic { get; set; }
        public bool IsSchoolPublic { get; set; }
        public string? SchoolClassId { get; set; }
        public SchoolClass? SchoolClass { get; set; }
    }
}
