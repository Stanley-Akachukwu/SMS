using SMS.Common.Dtos.Fees;
using SMS.Common.Dtos;
using SMS.ApiService.Repositories.Settings;

namespace SMS.ApiService.Repositories.Settings
{
    public interface IFeesSettingRepository
    {
        Task<ApiResponse<IEnumerable<FeesSettingDto>>?> GetFeesSettingsAsync(CancellationToken cancellationToken);
        Task<ApiResponse<FeesSettingDto>?> GetFeesSettingByIdAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<FeesSettingDto>?> UpdateFeesSettingAsync(FeesSettingDto dto, CancellationToken cancellationToken);
    }
}


