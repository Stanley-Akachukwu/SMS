using Microsoft.AspNetCore.Identity;
using SMS.ApiService.Persistence;
using SMS.Common.Dtos.Departments;
using SMS.Common.Dtos.Users;
using SMS.Common.Dtos;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Repositories.Auths;
using SMS.ApiService.Entities.Users;
using SMS.ApiService.Entities.Roles;
using Azure.Core;

namespace SMS.ApiService.Repositories.Users
{
    public class UserRepository(IAuthRepository _authRepository, AppDbContext _dbContext, HttpClient _httpClient) : IUserRepository
    {

        public async Task<ApiResponse<string>> CreateOrUpdateUserAsync(UserDto userDto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dbContext.Users
                  .FirstOrDefaultAsync(u => u.Email == userDto.UserName, cancellationToken);

                if (user == null)
                {
                    var rsp= await _authRepository.RegisterAsync(userDto);
                    if (rsp == null || rsp.Data == null)
                    {
                        return ApiResponse<string>.Failure("Failed to create user", StatusCodes.Status500InternalServerError);
                    }
                    user = rsp.Data;
                }

                var role = await _dbContext.Roles
                    .FirstOrDefaultAsync(r => r.Id == userDto.RoleId, cancellationToken);

                if (role == null)
                {
                    return ApiResponse<string>.Failure("Role was not found", StatusCodes.Status500InternalServerError);
                }

                var dept = await _dbContext.Departments
                    .FirstOrDefaultAsync(d => d.Id == userDto.DepartmentId, cancellationToken);

                if (dept == null)
                {
                    return ApiResponse<string>.Failure("Department was not found", StatusCodes.Status500InternalServerError);
                }

                user.RoleId = role.Id;
                user.DepartmentId = dept.Id;
                user.Email = userDto?.UserName!;
                user.FirstName = userDto?.FirstName;
                user.MiddleName = userDto?.MiddleName;
                user.SurName = userDto?.SurName;
                user.DateUpdated = DateTime.UtcNow;
                user.UpdatedByUserId = "System";  
                user.Description = userDto?.Description!;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

                string action = "created";
                int statusCode = StatusCodes.Status201Created;
                return ApiResponse<string>.Success($"Successfully {action}", statusCode);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ApiResponse<IEnumerable<UserDto>>> GetUsersAsync(CancellationToken cancellationToken)
        {
            try
            {

                var usersWithRoles = await _dbContext.Users
                                            .Include(u => u.Role)
                                            .Include(u => u.Department)
                                            .Include(u => u.UserProfile)
                                            .Select(p => new UserDto
                                            {
                                                DepartmentId = p.DepartmentId,
                                                Description = p.Description,
                                                FirstName = p.FirstName,
                                                MiddleName = p.MiddleName,
                                                Id = p.Id,
                                                RoleId = p.RoleId,
                                                SurName = p.SurName,
                                                UserName = p.Email,
                                            })
                                            .ToListAsync(cancellationToken);


                return ApiResponse<IEnumerable<UserDto>>.Success(usersWithRoles, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                var rsp = new ApiResponse<IEnumerable<UserDto>>();
                rsp.ErrorMessage = ex.Message;
                return rsp;
            }
        }
        public async Task<ApiResponse<UserDto>> GetUserInfoByEmailAsync(string email)
        {
            try
            {
                var userDto = await _dbContext.Users
                    .Where(p => p.Email == email)
                    .Select(p => new UserDto
                    {
                        DepartmentId = p.DepartmentId,
                        Description = p.Description,
                        FirstName = p.FirstName,
                        MiddleName = p.MiddleName,
                        Id = p.Id,
                        RoleId = p.RoleId,
                        SurName = p.SurName,
                        UserName = p.Email, 
                    })
                    .FirstOrDefaultAsync();

                if (userDto == null)
                    return ApiResponse<UserDto>.Failure("User not found", StatusCodes.Status404NotFound);

                return ApiResponse<UserDto>.Success(userDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserDto>.Failure($"Internal server error: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<IEnumerable<DepartmentDto>>?> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            var departments = await _dbContext.Departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                MenuSectionMap = d.MenuSectionMap,
            })
                .OrderBy(d => d.Name)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return departments.Any()
                ? ApiResponse<IEnumerable<DepartmentDto>>.Success(departments, StatusCodes.Status200OK)
                : ApiResponse<IEnumerable<DepartmentDto>>.Failure("No departments found", StatusCodes.Status404NotFound);
        }
    }
}
