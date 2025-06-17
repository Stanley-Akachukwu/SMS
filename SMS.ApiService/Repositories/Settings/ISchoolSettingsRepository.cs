using SMS.ApiService.Repositories.Settings;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Repositories.Settings
{
    public interface ISchoolSettingsRepository
    {
        Task<ApiResponse<IEnumerable<SchoolSettingDto>>?> GetSchoolSettingsAsync(CancellationToken cancellationToken);
        Task<ApiResponse<SchoolSettingDto>?> GetSchoolSettingByIdAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<SchoolSettingDto>?> UpdateSchoolSettingAsync(SchoolSettingDto dto, CancellationToken cancellationToken);
    }
}

