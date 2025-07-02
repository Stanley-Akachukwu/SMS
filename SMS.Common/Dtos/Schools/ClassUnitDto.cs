

using SMS.Common.Enums;

namespace SMS.Common.Dtos.Schools
{
    public class ClassUnitDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public SMSCoreAction Action { get; set; }
        public string? CreatedByUserId { get; set; } = string.Empty;
        public bool IsClassPublic { get; set; }
        public bool IsSchoolPublic { get; set; }
        public string? ClassId { get; set; }
        public string? ClassName { get; set; }
        public SchoolClassDto? Class { get; set; }
        public string? CodeName { get; set; } = "Super Man";
        public int Size { get; set; } = 0;
        public List<SchoolClassDto>? Classes { get; set; } = new();
    }
}
