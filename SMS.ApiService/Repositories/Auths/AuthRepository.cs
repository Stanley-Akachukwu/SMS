using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SMS.ApiService.Entities.Roles;
using SMS.ApiService.Entities.Users;
using SMS.ApiService.Helpers;
using SMS.ApiService.Persistence;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Auths;
using SMS.Common.Dtos.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SMS.ApiService.Repositories.Auths
{
    public class AuthRepository(AppDbContext context, IConfiguration configuration) : IAuthRepository
    {
        public async Task<ApiResponse<TokenResponseDto?>> LoginAsync(UserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.UserName);
            if (user is null)
            {
                return ApiResponse<TokenResponseDto?>.Failure("User was not found", StatusCodes.Status500InternalServerError);
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return ApiResponse<TokenResponseDto?>.Failure("Invalid password", StatusCodes.Status500InternalServerError);
            }

           var token = await CreateTokenResponse(user);
            return ApiResponse<TokenResponseDto?>.Success(token, StatusCodes.Status200OK);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User? user)
        {
            var tokenExpiringTime = DateTime.UtcNow.AddMinutes(5);
            return new TokenResponseDto
            {
                TokenExpired = DateTime.UtcNow.AddMinutes(5),
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user, tokenExpiringTime)
            };
        }

        public async Task<ApiResponse<User?>> RegisterAsync(UserDto request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.UserName))
            {
                return ApiResponse<User?>.Failure("User was not found", StatusCodes.Status500InternalServerError);
            }

            var user = new User();
            string password = PasswordGenerator.GeneratePassword();
            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, password);

            user.Id = Ulid.NewUlid().ToString();
            user.Email = request?.UserName!;
            user.PasswordHash = hashedPassword;
            user.DateCreated = DateTime.UtcNow;
            user.Description = "System User";
            user.RoleId = request?.Id;
            user.DepartmentId = request?.DepartmentId;
            user.Email = request?.UserName!;
            user.FirstName = request?.FirstName;
            user.MiddleName = request?.MiddleName;
            user.SurName = request?.SurName;
            user.DateUpdated = DateTime.UtcNow;
            user.UpdatedByUserId = "System";


            context.Users.Add(user);
            await context.SaveChangesAsync();

            return ApiResponse<User?>.Success(user, StatusCodes.Status200OK);
        }

       
        public async Task<ApiResponse<TokenResponseDto>?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request);
            if (user is null)
                return ApiResponse<TokenResponseDto>.Failure("Invalid token", StatusCodes.Status500InternalServerError);


            var token = await CreateTokenResponse(user);
            return ApiResponse<TokenResponseDto>.Success(token, StatusCodes.Status200OK);
        }
        private async Task<User?> ValidateRefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            //var user = await context.Users.FirstOrDefaultAsync(u=>u.RefreshToken == refreshToken);
            //if (user is null || user.RefreshToken != refreshToken
            //   || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(User user, DateTime tokenExpiringTime)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = tokenExpiringTime;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        private string CreateToken(User? user)
        {
            var fullName = (user?.FirstName ?? "") + " " + (user?.MiddleName ?? "") + " " + (user?.SurName ?? "");
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(fullName))
                claims.Add(new Claim(ClaimTypes.Name, fullName));

            if (!string.IsNullOrWhiteSpace(user?.Email))
                claims.Add(new Claim("Email", user.Email));

            if (user?.Id != null)
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            if (!string.IsNullOrWhiteSpace(user?.Role?.Name))
                claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
           
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
