using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Persistence;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Repositories.Settings
{
    public class SchoolSettingsRepository(AppDbContext _dbContext) : ISchoolSettingsRepository
    {
        public async Task<ApiResponse<IEnumerable<SchoolSettingDto>>?> GetSchoolSettingsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var settings = await _dbContext.SchoolSettings
                        .Select(setting => new SchoolSettingDto
                        {
                            Id = setting.Id,
                            Name = setting.Name,
                            Description = setting.Description,
                            BreakEndTime = setting.BreakEndTime,
                            BreakStartTime = setting.BreakStartTime,
                            ClassStartTime = setting.ClassStartTime,
                            DismissalTime = setting.DismissalTime,
                            MidTermBreak = setting.MidTermBreak,
                        })
                        .ToListAsync(cancellationToken);
                return ApiResponse<IEnumerable<SchoolSettingDto>>.Success(settings, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                var rsp = new ApiResponse<IEnumerable<SchoolSettingDto>> ();
                rsp.ErrorMessage = ex.Message;
                return rsp;
            }
        }
        public async Task<ApiResponse<SchoolSettingDto>?> GetSchoolSettingByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var setting = await _dbContext.SchoolSettings
                    .Where(s => s.Id == id)
                    .Select(s => new SchoolSettingDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        BreakEndTime = s.BreakEndTime,
                        BreakStartTime = s.BreakStartTime,
                        ClassStartTime = s.ClassStartTime,
                        DismissalTime = s.DismissalTime,
                        MidTermBreak = s.MidTermBreak,
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (setting == null)
                    return ApiResponse<SchoolSettingDto>.Failure("School setting not found.", StatusCodes.Status404NotFound);

                return ApiResponse<SchoolSettingDto>.Success(setting, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<SchoolSettingDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SchoolSettingDto>?> UpdateSchoolSettingAsync(SchoolSettingDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var setting = await _dbContext.SchoolSettings.FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);
                if (setting == null)
                    return ApiResponse<SchoolSettingDto>.Failure("School setting not found.", StatusCodes.Status404NotFound);

                setting.Name = dto.Name;
                setting.Description = dto.Description!;
                setting.BreakEndTime = dto.BreakEndTime;
                setting.BreakStartTime = dto.BreakStartTime;
                setting.ClassStartTime = dto.ClassStartTime;
                setting.DismissalTime = dto.DismissalTime;
                setting.MidTermBreak = dto.MidTermBreak;

                await _dbContext.SaveChangesAsync(cancellationToken);

                // Optionally, map back to DTO
                var updatedDto = new SchoolSettingDto
                {
                    Id = setting.Id,
                    Name = setting.Name,
                    Description = setting.Description,
                    BreakEndTime = setting.BreakEndTime,
                    BreakStartTime = setting.BreakStartTime,
                    ClassStartTime = setting.ClassStartTime,
                    DismissalTime = setting.DismissalTime,
                    MidTermBreak = setting.MidTermBreak,
                };

                return ApiResponse<SchoolSettingDto>.Success(updatedDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<SchoolSettingDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
        
    }
}
