

using SMS.Common.Enums;

namespace SMS.Common.Dtos.Schools
{
    public class SchoolDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SettingId { get; set; }
        public SchoolSettingDto Setting { get; set; } = new();
        public List<SchoolSettingDto>? Settings { get; set; } = new();
        public SMSCoreAction Action { get; set; }
        public List<SchoolClassDto> Classes { get; set; } = new();
        public string? CreatedByUserId { get; set; } = string.Empty;
    }

    
}
