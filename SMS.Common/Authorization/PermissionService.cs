using System.Text.Json;
using SMS.Common.Enums;
using SMS.Common.Dtos.Roles;
using SMS.Common.Dtos;

namespace SMS.Common.Authorization
{
    public class PermissionService : IPermissionService
    {
        
        private Dictionary<string, List<Permission>> _userPermissions;
        private async Task<Dictionary<string, List<Permission>>> GetRolePermissionsAsync(string? id, CancellationToken cancellationToken)
        {
            id = string.IsNullOrEmpty(id) ? "0" : id;
            try
            {
                var apiResponse = new ApiResponse<IEnumerable<RolePermissionDto>>();
                var client = new HttpClient();
                using (var response = await client.GetAsync($"https://localhost:7220/api/Roles/permissions/{id}", cancellationToken))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();

                        apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<RolePermissionDto>>>(body, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        var permissions = apiResponse.Data.Select(p=> p.PermissionName).ToList();
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
        public async Task<List<Permission>> GetPermissionsAsync(string? userId)
        {
            var userPermissions = await GetRolePermissionsAsync(userId, CancellationToken.None);
            _userPermissions.TryGetValue(userId, out var permissions);
            return permissions;
        }
        public async Task<bool> HasPermissionAsync(Permission permission, string? userId)
        {
            return  true;
            //using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            //var permissionsResult = await GetRolePermissionsAsync(userId, cts.Token);
            //var permissions = permissionsResult.FirstOrDefault().Value;
            //return permissions.Contains(permission);
        }
    }

}
