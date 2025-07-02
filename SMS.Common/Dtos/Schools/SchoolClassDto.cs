

using SMS.Common.Enums;

namespace SMS.Common.Dtos.Schools
{
    public class SchoolClassDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public SMSCoreAction Action { get; set; }
        public string? CreatedByUserId { get; set; } = string.Empty;
        public string? SchoolId { get; set; }
        public string? SchoolName { get; set; }
        public SchoolDto? School { get; set; }
        public List<SchoolDto>? Schools { get; set; } = new();
        public string? SettingId { get; set; }
        public ClassSettingDto Setting { get; set; } = new();
        public List<ClassSettingDto>? Settings { get; set; } = new();
        public ICollection<ClassUnitDto> Groups { get; set; } = new List<ClassUnitDto>();
    }
}
