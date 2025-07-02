using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Settings
{
    public class FeesSetting : BaseEntity<string>
    {
        [MaxLength(128)]
        public string? Name { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<FeesSettingBreakdown>? FeesBreakDown { get; set; } = new List<FeesSettingBreakdown>();
    }
}
