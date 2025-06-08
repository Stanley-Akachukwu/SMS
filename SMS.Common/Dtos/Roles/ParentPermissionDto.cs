
namespace SMS.Common.Dtos.Roles
{
    public class ParentPermissionDto
    {
        public string Id { get; set; }
        public int ParentId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
