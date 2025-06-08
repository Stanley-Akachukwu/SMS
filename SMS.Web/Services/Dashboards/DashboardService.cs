using SMS.Common.Dtos;
using SMS.Common.Dtos.Dashboards;
using System.Net;
using System.Text.Json;
namespace SMS.Web.Services.Dashboards
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<List<DashboardItemDto>>> GetDashboardsAsync(string? department, CancellationToken cancellationToken)
        {
            try
            {
                using var response = await _httpClient.GetAsync($"dashboards/{department}", cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new ApiResponse<List<DashboardItemDto>>
                    {
                        IsSuccess = false,
                        StatusCode = 401,
                        ErrorMessage = "Unauthorized"
                    };
                }

                // Handle success
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync(cancellationToken);

                    // Check for empty or null body
                    if (string.IsNullOrWhiteSpace(body))
                    {
                        return ApiResponse<List<DashboardItemDto>>.Failure("Empty response body", (int)response.StatusCode);
                    }

                    try
                    {
                        var resultRsp = JsonSerializer.Deserialize<ApiResponse<List<DashboardItemDto>>>(body, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return resultRsp ?? ApiResponse<List<DashboardItemDto>>.Failure("Deserialization returned null", (int)response.StatusCode);
                    }
                    catch (JsonException ex)
                    {
                        return ApiResponse<List<DashboardItemDto>>.Failure($"Invalid JSON: {ex.Message}", (int)response.StatusCode);
                    }
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync(cancellationToken);
                    // Optional: log error response content here

                    return ApiResponse<List<DashboardItemDto>>.Failure(
                        $"API call failed with status {(int)response.StatusCode}: {error}",
                        (int)response.StatusCode
                    );
                }
            }
            catch (HttpRequestException ex)
            {
                return ApiResponse<List<DashboardItemDto>>.Failure($"Request failed: {ex.Message}", 0);
            }
            catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                return ApiResponse<List<DashboardItemDto>>.Failure("Request was cancelled.", 0);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DashboardItemDto>>.Failure($"Unexpected error: {ex.Message}", 0);
            }
        }

    }
}
