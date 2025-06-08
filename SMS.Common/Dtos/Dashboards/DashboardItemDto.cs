using SMS.Common.Dtos.Fluents;

namespace SMS.Common.Dtos.Dashboards
{
    public class DashboardItemDto: FluentDto
    {
        public string Title { get; set; }             // e.g., "Students"
        public string Href { get; set; }              // e.g., "students"
        public List<DashboardMetric> Metrics { get; set; } = new();
    }
}
