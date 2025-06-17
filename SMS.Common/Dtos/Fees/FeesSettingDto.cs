
using System.ComponentModel.DataAnnotations;

namespace SMS.Common.Dtos.Fees
{
    public class FeesSettingDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        public List<FeesSettingBreakdownDto>? FeesBreakDown { get; set; } = new List<FeesSettingBreakdownDto>();
    }
}
