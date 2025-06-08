using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Entities.Users;
using SMS.ApiService.Repositories.Auths;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Auths;
using SMS.Common.Dtos.Users;

namespace SMS.ApiService.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthRepository authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user is null)
                return BadRequest("Username already exists.");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            var result = await authService.LoginAsync(request);
            if (result is null)
                return BadRequest("Invalid username or password.");

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<TokenResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokensAsync(request);
            if (result == null || result?.Data?.AccessToken == null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are and admin!");
        }
    }
}
