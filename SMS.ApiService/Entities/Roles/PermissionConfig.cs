namespace SMS.ApiService.Entities.Roles
{
    public class PermissionConfig : BaseEntity<int>
    {
        public string? Name { get; set; }
        public int? ParentId { get; set; }
        public bool? StaffAssign { get; set; }
        public bool? ParentAssign { get; set; }
        public bool? AdminAssign { get; set; }
        public bool? IsDefault { get; set; }
    }
}
