using System.ComponentModel.DataAnnotations;
namespace SMS.ApiService.Entities.Settings
{
    public class FeesSettingBreakdown : BaseEntity<string>
    {
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(150)]
        public string? FeesSettingId { get; set; }
        public FeesSetting? FeesSetting { get; set; }
    }
}
