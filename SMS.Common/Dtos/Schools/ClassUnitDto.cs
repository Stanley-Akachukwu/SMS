 

namespace SMS.Common.Dtos.Schools
{
    public class ClassUnitDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public bool IsClassPublic { get; set; }
        public bool IsSchoolPublic { get; set; }
        public string? SchoolClassId { get; set; }
        public SchoolClassDto? SchoolClass { get; set; }
    }
}
