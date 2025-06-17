
using System.ComponentModel.DataAnnotations;

namespace SMS.Common.Dtos.Schools
{
    public class ClassSettingDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Max. Population Size")]
        public int MaxPopulationSize { get; set; }
        [Display(Name = "Max. Number of Teacher")]
        public int MinNumberOfTeachers { get; set; }
    }
}
