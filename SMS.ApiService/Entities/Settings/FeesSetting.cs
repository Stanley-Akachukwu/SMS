namespace SMS.ApiService.Entities.Settings
{
    public class FeesSetting : BaseEntity<string>
    {
        public string? Name { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<FeesSettingBreakdown>? FeesBreakDown { get; set; } = new List<FeesSettingBreakdown>();
    }
}
