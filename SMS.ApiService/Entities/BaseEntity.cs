using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;

        [MaxLength(128)]
        public string? CreatedByUserId { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTime.Now;
        [MaxLength(128)]
        public string? UpdatedByUserId { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        [MaxLength(128)]
        public string? DeletedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
    }
}
