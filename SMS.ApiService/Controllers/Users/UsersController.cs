using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.Users;
using SMS.Common.Dtos.Users;


namespace SMS.ApiService.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;
        public UsersController(IUserRepository repository)
        {
            _repository = repository;   
        }

        [HttpGet("info/{email}")]
        [Authorize]
        public async Task<ActionResult> GetInfoAsync(string email)
        {
            return Ok( await _repository.GetUserInfoByEmailAsync(email));
        }

        [HttpPost("user")]
        public async Task<ActionResult> CreateOrUpdateUserAsync([FromBody] UserDto userSetupDto, CancellationToken cancellationToken)
        {
            return Ok(await _repository.CreateOrUpdateUserAsync(userSetupDto, cancellationToken));
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetUsersAsync(CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetUsersAsync(cancellationToken));
        }
        [HttpGet("departments")]
        public async Task<ActionResult> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetDepartmentsAsync(cancellationToken));
        }

        [Authorize]
        [HttpPost("delete/{id}")]
        public async Task<ActionResult> DeleteUserAsync(string id, CancellationToken cancellationToken)
        {
            return Ok(await _repository.DeleteUserAsync(id, cancellationToken));
        }
    }
}
