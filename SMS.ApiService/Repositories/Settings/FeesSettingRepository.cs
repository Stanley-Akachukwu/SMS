using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Persistence;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Fees;

namespace SMS.ApiService.Repositories.Settings
{
    public class FeesSettingRepository(AppDbContext _dbContext) : IFeesSettingRepository
    {
        public async Task<ApiResponse<IEnumerable<FeesSettingDto>>?> GetFeesSettingsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var settings = await _dbContext.FeesSettings
                    .Select(setting => new FeesSettingDto
                    {
                        Id = setting.Id,
                        Name = setting.Name,
                        Description = setting.Description,
                        TotalAmount = setting.TotalAmount,
                    })
                    .ToListAsync(cancellationToken);

                return ApiResponse<IEnumerable<FeesSettingDto>>.Success(settings, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<FeesSettingDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<FeesSettingDto>?> GetFeesSettingByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var setting = await _dbContext.FeesSettings
                    .Where(s => s.Id == id).Include(f => f.FeesBreakDown)
                    .Select(s => new FeesSettingDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        TotalAmount = s.TotalAmount,
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (setting == null)
                    return ApiResponse<FeesSettingDto>.Failure("Fees setting not found.", StatusCodes.Status404NotFound);

                return ApiResponse<FeesSettingDto>.Success(setting, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<FeesSettingDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<FeesSettingDto>?> UpdateFeesSettingAsync(FeesSettingDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var setting = await _dbContext.FeesSettings
                    .Include(f => f.FeesBreakDown)
                    .FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);

                if (setting == null)
                    return ApiResponse<FeesSettingDto>.Failure("Fees setting not found.", StatusCodes.Status404NotFound);

                setting.Name = dto.Name;
                setting.TotalAmount = dto.TotalAmount;

                // Optionally update breakdowns here if needed

                await _dbContext.SaveChangesAsync(cancellationToken);

                var updatedDto = new FeesSettingDto
                {
                    Id = setting.Id,
                    Name = setting.Name,
                    TotalAmount = setting.TotalAmount,
                    FeesBreakDown = setting.FeesBreakDown?.Select(b => new FeesSettingBreakdownDto
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Amount = b.Amount
                    }).ToList()
                };

                return ApiResponse<FeesSettingDto>.Success(updatedDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<FeesSettingDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
