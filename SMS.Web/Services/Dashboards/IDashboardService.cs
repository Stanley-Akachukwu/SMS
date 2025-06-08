using SMS.Common.Dtos;
using SMS.Common.Dtos.Dashboards;

namespace SMS.Web.Services.Dashboards
{
    public interface IDashboardService
    {
        Task<ApiResponse<List<DashboardItemDto>>> GetDashboardsAsync(string department, CancellationToken cancellationToken);
    }
}
