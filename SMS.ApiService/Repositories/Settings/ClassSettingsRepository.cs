using SMS.ApiService.Persistence;
using SMS.Common.Dtos.Schools;
using SMS.Common.Dtos;
using Microsoft.EntityFrameworkCore;

namespace SMS.ApiService.Repositories.Settings
{
    public class ClassSettingsRepository(AppDbContext _dbContext) : IClassSettingsRepository
    {
        public async Task<ApiResponse<IEnumerable<ClassSettingDto>>?> GetClassSettingsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var settings = await _dbContext.ClassSettings
                    .Select(setting => new ClassSettingDto
                    {
                        Id = setting.Id,
                        Name = setting.Name,
                        Description = setting.Description,
                        MaxPopulationSize = setting.MaxPopulationSize,
                        MinNumberOfTeachers = setting.MinNumberOfTeachers
                    })
                    .ToListAsync(cancellationToken);

                return ApiResponse<IEnumerable<ClassSettingDto>>.Success(settings, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ClassSettingDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ClassSettingDto>?> GetClassSettingByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var setting = await _dbContext.ClassSettings
                    .Where(s => s.Id == id)
                    .Select(s => new ClassSettingDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        MaxPopulationSize = s.MaxPopulationSize,
                        MinNumberOfTeachers = s.MinNumberOfTeachers
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (setting == null)
                    return ApiResponse<ClassSettingDto>.Failure("Class setting not found.", StatusCodes.Status404NotFound);

                return ApiResponse<ClassSettingDto>.Success(setting, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClassSettingDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ClassSettingDto>?> UpdateClassSettingAsync(ClassSettingDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var setting = await _dbContext.ClassSettings.FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);
                if (setting == null)
                    return ApiResponse<ClassSettingDto>.Failure("Class setting not found.", StatusCodes.Status404NotFound);

                setting.Name = dto.Name;
                setting.MaxPopulationSize = dto.MaxPopulationSize;
                setting.MinNumberOfTeachers = dto.MinNumberOfTeachers;

                await _dbContext.SaveChangesAsync(cancellationToken);

                var updatedDto = new ClassSettingDto
                {
                    Id = setting.Id,
                    Name = setting.Name,
                    MaxPopulationSize = setting.MaxPopulationSize,
                    MinNumberOfTeachers = setting.MinNumberOfTeachers
                };

                return ApiResponse<ClassSettingDto>.Success(updatedDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClassSettingDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
