

namespace SMS.Common.Dtos.Schools
{
    public class SchoolSettingDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public TimeSpan? ClassStartTime { get; set; }
        public TimeSpan? BreakStartTime { get; set; }
        public TimeSpan? BreakEndTime { get; set; }
        public TimeSpan? DismissalTime { get; set; }
        public DateOnly? MidTermBreak { get; set; }

        public ICollection<SchoolDto> Schools { get; set; } = new List<SchoolDto>();
    }
}
