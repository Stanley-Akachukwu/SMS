namespace SMS.Common.Dtos.Roles
{
    public class RolePermissionDto
    {
        public int ParentId { get; set; }
        public string? Id { get; set; }
        public string? RoleId { get; set; }
        public string? Role { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public string? Description { get; set; }
        public bool Assigned { get; set; }
    }
}
