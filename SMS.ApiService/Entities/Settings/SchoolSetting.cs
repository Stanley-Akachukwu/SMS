using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Settings
{
    public class SchoolSetting: BaseEntity<string>
    {
        [Required]
        [MaxLength(128)]
        public string? Name { get; set; }
        public TimeSpan? ClassStartTime { get; set; }
        public TimeSpan? BreakStartTime { get; set; }
        public TimeSpan? BreakEndTime { get; set; }
        public TimeSpan? DismissalTime { get; set; }
        public DateOnly? MidTermBreak { get; set; }

    }
}
