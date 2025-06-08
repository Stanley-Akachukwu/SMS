using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Common.Authorization;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Dashboards;
using SMS.Common.Enums;


namespace SMS.ApiService.Controllers.Dashboards
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        [Authorize]
        [HasPermission(Permission.CanViewAllDashboard)]
        [HttpGet("{department?}")]
        public async Task<ActionResult> GetDashboardsAsync(string? department)
        {
            var mockData = new List<DashboardItemDto>();
            var rsp = new ApiResponse<List<DashboardItemDto>>();
            try
            {
                mockData = new List<DashboardItemDto>
            {
                new DashboardItemDto()
                {
                    Title = "Students",
                    Href = "students",
                    IconName = "People",
                    IconSet = "Regular",
                    IconSize = "Size16",
                    IconColor = "Warning",
                    Metrics = new()
                    {
                        new() { Label = "Total", Value = "1,200" },
                        new() { Label = "Active", Value = "1,150" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Teachers",
                    Href = "teachers",
                    IconName = "PersonAvailable",
                    IconSet = "Regular",
                    IconSize = "Size16",
                    IconColor = "Success",
                    Metrics = new()
                    {
                        new() { Label = "Total", Value = "1,200" },
                        new() { Label = "Departments", Value = "8" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Workers",
                    Href = "non-academic-staff",
                    IconName = "PersonAvailable",
                    IconSet = "Regular",
                    IconSize = "Size16",
                    IconColor = "Success",
                    Metrics = new()
                    {
                        new() { Label = "Total", Value = "1,200" },
                        new() { Label = "Departments", Value = "8" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Classes",
                    Href = "classes",
                    IconName = "Class",
                    IconSet = "Filled",
                    IconSize = "Size20",
                    IconColor = "Custom",
                    Metrics = new()
                    {
                        new() { Label = "Total", Value = "1,200" },
                        new() { Label = "Available Today", Value = "45" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Results",
                    Href = "results",
                    IconName = "DocumentBulletList",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Success",
                    Metrics = new()
                    {
                        new() { Label = "Pending", Value = "8" },
                        new() { Label = "Published", Value = "3" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Library",
                    Href = "library",
                    IconName = "BookOpen",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Neutral",
                    Metrics = new()
                    {
                        new() { Label = "Books", Value = "5,000" },
                        new() { Label = "Issued", Value = "1,200" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Finance",
                    Href = "finance",
                    IconName = "Money",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Warning",
                    Metrics = new()
                    {
                        new() { Label = "Total Income", Value = "₦1.2M" },
                        new() { Label = "Expenses", Value = "₦800k" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Attendance",
                    Href = "attendance",
                    IconName = "CalendarCheckmark",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Custom",
                    Metrics = new()
                    {
                        new() { Label = "Today", Value = "97%" },
                        new() { Label = "Monthly Avg", Value = "95%" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Assignments",
                    Href = "assignments",
                    IconName = "ScanDash",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Accent",
                    Metrics = new()
                    {
                        new() { Label = "Due Today", Value = "10" },
                        new() { Label = "Pending", Value = "45" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Events",
                    Href = "events",
                    IconName = "CalendarLtr",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Success",
                    Metrics = new()
                    {
                        new() { Label = "Upcoming", Value = "5" },
                        new() { Label = "Today", Value = "1" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Messages",
                    Href = "messages",
                    IconName = "Mail",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Neutral",
                    Metrics = new()
                    {
                        new() { Label = "Unread", Value = "42" },
                        new() { Label = "Sent Today", Value = "120" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Fee Management",
                    Href = "fee-management",
                    IconName = "Briefcase",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Success",
                    Metrics = new()
                    {
                        new() { Label = "Fees Collected", Value = "₦250,000" },
                        new() { Label = "Outstanding", Value = "₦15,000" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Transport",
                    Href = "transport",
                    IconName = "VehicleCar",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Warning",
                    Metrics = new()
                    {
                        new() { Label = "Buses", Value = "15" },
                        new() { Label = "Routes Active", Value = "12" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Hostel",
                    Href = "hostel",
                    IconName = "Home",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Accent",
                    Metrics = new()
                    {
                        new() { Label = "Residents", Value = "350" },
                        new() { Label = "Vacant Rooms", Value = "28" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Support",
                    Href = "complaint-support",
                    IconName = "Chat",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Error",
                    Metrics = new()
                    {
                        new() { Label = "Tickets Open", Value = "9" },
                        new() { Label = "Resolved", Value = "34" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Uniforms",
                    Href = "school-uniform-sets",
                    IconName = "FolderMultiple",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Success",
                    Metrics = new()
                    {
                        new() { Label = "Male", Value = "360" },
                        new() { Label = "Female", Value = "340" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "PTA Projects",
                    Href = "community-projects",
                    IconName = "MoneySettings",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Warning",
                    Metrics = new()
                    {
                        new() { Label = "Total", Value = "10" },
                        new() { Label = "Active", Value = "2" }
                    }
                },
                new DashboardItemDto()
                {
                    Title = "Donations",
                    Href = "donations",
                    IconName = "GiftOpen",
                    IconSet = "Filled",
                    IconSize = "Size16",
                    IconColor = "Warning",
                    Metrics = new()
                    {
                        new() { Label = "Redeemed", Value = "10" },
                        new() { Label = "Pending", Value = "2" }
                    }
                }
            };
                rsp = new ApiResponse<List<DashboardItemDto>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Data = mockData
                };
            }
            catch (Exception exp)
            {

                throw;
            }

            return Ok(rsp);
        }
    }

}
