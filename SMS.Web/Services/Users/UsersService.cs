using SMS.Common.Dtos;
using SMS.Common.Dtos.Departments;
using SMS.Common.Dtos.Users;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SMS.Web.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        public UsersService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
         
        public async Task<ApiResponse<string>> CreateOrUpdateUserAsync(UserDto userSetup, CancellationToken cancellationToken)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {

                var requestJson = JsonSerializer.Serialize(userSetup);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_httpClient.BaseAddress}users/user"),
                    Content = new StringContent(requestJson)
                    {
                        Headers =
                                {
                                    ContentType = new MediaTypeHeaderValue("application/json")
                                }
                    }
                };
                using (var response = await _httpClient.SendAsync(request))
                {
                    var body = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        // Log or return the error details
                        return ApiResponse<string>.Failure(
                            $"API Error: {(int)response.StatusCode} {response.ReasonPhrase}. Response: {body}",
                            (int)response.StatusCode
                        );
                    }
                    apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return apiResponse;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure($"Request failed: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
            
        }

        public Task<ApiResponse<string>> DeleteRoleAsync(string id, CancellationToken cancellationToken)
        {
            return Task.FromResult<ApiResponse<string>>(ApiResponse<string>.Failure("Not Implemented", 501));
        }

        public async Task<ApiResponse<IEnumerable<UserDto>>?> GetUsersAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var response = await _httpClient.GetAsync("users/all", cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();

                        var resultRsp = JsonSerializer.Deserialize<ApiResponse<IEnumerable<UserDto>>>(body, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return resultRsp;
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        //log error
                        return ApiResponse<IEnumerable<UserDto>>.Failure("Internal Server error", (int)response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return ApiResponse<IEnumerable<UserDto>>.Failure($"Request failed: {ex.Message}", 0);
            }
        }

        public async Task<ApiResponse<IEnumerable<DepartmentDto>>?> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var response = await _httpClient.GetAsync("users/departments", cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();

                        var resultRsp = JsonSerializer.Deserialize<ApiResponse<IEnumerable<DepartmentDto>>>(body, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return resultRsp;
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        //log error
                        return ApiResponse<IEnumerable<DepartmentDto>>.Failure("Internal Server error", (int)response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return ApiResponse<IEnumerable<DepartmentDto>>.Failure($"Request failed: {ex.Message}", 0);
            }
        }
    }
}
