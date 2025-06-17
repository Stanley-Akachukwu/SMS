 

namespace SMS.Common.Dtos.Schools
{
    public class SchoolClassDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? SchoolId { get; set; }
        public SchoolDto? School { get; set; }
        public ICollection<ClassUnitDto> Groups { get; set; } = new List<ClassUnitDto>();
    }
}
