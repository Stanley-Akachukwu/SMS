using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Settings
{
    public class ClassSetting : BaseEntity<string>
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        public int MaxPopulationSize { get; set; }
        public int MinNumberOfTeachers { get; set; }
    }
}
