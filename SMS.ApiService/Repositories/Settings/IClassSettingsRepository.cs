using SMS.Common.Dtos.Schools;
using SMS.Common.Dtos;

namespace SMS.ApiService.Repositories.Settings
{
    public interface IClassSettingsRepository
    {
        Task<ApiResponse<IEnumerable<ClassSettingDto>>?> GetClassSettingsAsync(CancellationToken cancellationToken);
        Task<ApiResponse<ClassSettingDto>?> GetClassSettingByIdAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<ClassSettingDto>?> UpdateClassSettingAsync(ClassSettingDto dto, CancellationToken cancellationToken);
    }

}
