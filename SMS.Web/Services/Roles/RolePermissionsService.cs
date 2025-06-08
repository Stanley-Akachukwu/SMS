using SMS.Common.Dtos;
using SMS.Common.Dtos.Roles;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SMS.Web.Services.Roles
{
    public class RolePermissionsService : IRolePermissionsService
    {
        private readonly HttpClient _httpClient;

        public RolePermissionsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
 
        public async Task<ApiResponse<string>> CreateOrUpdateRoleAsync(RoleDto role, CancellationToken cancellationToken)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var requestJson = JsonSerializer.Serialize(role);

                //var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_httpClient.BaseAddress}Roles/create"),
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
                    return ApiResponse<string>.Success("Created successfully", (int)response.StatusCode);
                }

            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure($"Request failed: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<IEnumerable<RoleDto>>?> GetRolesAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (var response = await _httpClient.GetAsync("roles/all", cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();

                        var resultRsp = JsonSerializer.Deserialize<ApiResponse<IEnumerable<RoleDto>>>(body, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return resultRsp;   
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        //log error
                        return ApiResponse<IEnumerable<RoleDto>>.Failure("Internal Server error", (int)response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return ApiResponse<IEnumerable<RoleDto>>.Failure($"Request failed: {ex.Message}", 0);
            }
        }

        public async Task<ApiResponse<IEnumerable<RolePermissionDto>>?> GetRolePermissionsAsync(string? id, CancellationToken cancellationToken)
        {
          id =  string.IsNullOrEmpty(id)? "0":id;
            try
            {
                var apiResponse = new ApiResponse<IEnumerable<RolePermissionDto>>();
                using (var response = await _httpClient.GetAsync($"roles/permissions/{id}", cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();

                        apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<RolePermissionDto>>>(body, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        return apiResponse;
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        //log error
                        return ApiResponse<IEnumerable<RolePermissionDto>>.Failure("Internal Server error", (int)response.StatusCode);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return ApiResponse<IEnumerable<RolePermissionDto>>.Failure($"Request failed: {ex.Message}", 0);
            }
        }

        public async Task<ApiResponse<string>> DeleteRoleAsync(string id, CancellationToken cancellationToken)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{_httpClient.BaseAddress}Roles/delete/{id}"),
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
                    response.EnsureSuccessStatusCode();
                    var resultRsp = JsonSerializer.Deserialize<ApiResponse<string>>(body, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return resultRsp;
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure($"Request failed: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }
    }
}
