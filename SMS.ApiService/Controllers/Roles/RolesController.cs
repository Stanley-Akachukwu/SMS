using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.Roles;
using SMS.Common.Authorization;
using SMS.Common.Dtos.Roles;
using SMS.Common.Enums;


namespace SMS.ApiService.Controllers.Roles
{
    //---------------------------------------------------------REDIS
    //[Route("api/[controller]")]
    //[ApiController]
    //public class RolesController : ControllerBase
    //{
    //    private readonly IRoleRepository _repository;
    //    private readonly IRedisCacheService _cache;
    //    private const string RolesCacheKey = "roles:all";

    //    public RolesController(IRoleRepository repository, IRedisCacheService cache)
    //    {
    //        _repository = repository;
    //        _cache = cache;
    //    }

    //    [HttpPost("create")]
    //    public async Task<ActionResult> CreateRoleAsync([FromBody] RoleDto role, CancellationToken cancellationToken)
    //    {
    //        var result = await _repository.CreateOrUpdateRoleAsync(role, cancellationToken);
    //        await _cache.RemoveAsync(RolesCacheKey); // Invalidate cache
    //        return Ok(result);
    //    }

    //    [HttpGet("all")]
    //    public async Task<ActionResult> GetRolesAsync(CancellationToken cancellationToken)
    //    {
    //        var cachedRoles = await _cache.GetAsync<IEnumerable<RoleDto>>(RolesCacheKey);
    //        if (cachedRoles != null)
    //        {
    //            return Ok(cachedRoles);
    //        }

    //        var roles = await _repository.GetRolesAsync(cancellationToken);
    //        await _cache.SetAsync(RolesCacheKey, roles, TimeSpan.FromMinutes(10));
    //        return Ok(roles);
    //    }

    //    [HttpGet("permissions/{id}")]
    //    public async Task<ActionResult> GetRolePermissionsAsync(string id, CancellationToken cancellationToken)
    //    {
    //        var cacheKey = $"role:permissions:{id}";
    //        var cached = await _cache.GetAsync<IEnumerable<string>>(cacheKey);
    //        if (cached != null)
    //        {
    //            return Ok(cached);
    //        }

    //        var permissions = await _repository.GetPermissionsByRoleIdAsync(id, cancellationToken);
    //        await _cache.SetAsync(cacheKey, permissions, TimeSpan.FromMinutes(10));
    //        return Ok(permissions);
    //    }

    //    [HttpPost("delete/{id}")]
    //    public async Task<ActionResult> DeleteRolePermissionsAsync(string id, CancellationToken cancellationToken)
    //    {
    //        var result = await _repository.DeleteRole(id, cancellationToken);
    //        await _cache.RemoveAsync(RolesCacheKey);
    //        await _cache.RemoveAsync($"role:permissions:{id}");
    //        return Ok(result);
    //    }
    //}
    //---------------------------------------------------------REDIS



    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _repository;
        public RolesController(IRoleRepository repository)
        {
            _repository = repository;
        }


        [Authorize]
        [HasPermission(Permission.CanUpdatePermisions)]

        [HttpPost("create")]
        public async Task<ActionResult> CreateRoleAsync([FromBody] RoleDto role, CancellationToken cancellationToken)
        {
            return Ok(await _repository.CreateOrUpdateRoleAsync(role, cancellationToken));
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult> GetRolesAsync(CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetRolesAsync(cancellationToken));
        }

        [HttpGet("permissions/{id}")]
        public async Task<ActionResult> GetRolePermissionsAsync(string id, CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetPermissionsByRoleIdAsync(id, cancellationToken));
        }

        [Authorize]
        [HasPermission(Permission.CanDeletePermisions)]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteRolePermissionsAsync(string id, CancellationToken cancellationToken)
        {
            return Ok(await _repository.DeleteRole(id, cancellationToken));
        }
    }
}
