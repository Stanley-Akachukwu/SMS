using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text.Json;
using SMS.Common.Enums;
using SMS.Common.Dtos.Roles;
using SMS.Common.Dtos;

namespace SMS.Common.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        
        private readonly HttpClient _httpClient;
        public PermissionHandler(IPermissionService permissionService, HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,PermissionRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return;
            }
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            
            var permissionsResult = await GetRolePermissionsAsync(userId, cts.Token);
            var permissions = permissionsResult.FirstOrDefault().Value;

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return;

        }

        private async Task<Dictionary<string, List<Permission>>> GetRolePermissionsAsync(string? id, CancellationToken cancellationToken)
        {
            id = string.IsNullOrEmpty(id) ? "0" : id;
            Dictionary<string, List<Permission>> _userPermissions;
            try
            {
                var apiResponse = new ApiResponse<IEnumerable<RolePermissionDto>>();
                using (var response = await _httpClient.GetAsync($"https://localhost:7386/api/roles/permissions/{id}", cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();

                        apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<RolePermissionDto>>>(body, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        var permissions = apiResponse.Data.Select(p => p.PermissionName).ToList();
                        _userPermissions = new Dictionary<string, List<Permission>>
                        {
                            { id, permissions.Select(p => (Permission)Enum.Parse(typeof(Permission), p)).ToList() }
                        };

                        return _userPermissions;
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        //log error
                        return new Dictionary<string, List<Permission>>
                        {
                            { id, new List<Permission> { Permission.None } }
                        };
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return new Dictionary<string, List<Permission>>
                        {
                            { id, new List<Permission> { Permission.None } }
                        };
            }
        }
    }

}
