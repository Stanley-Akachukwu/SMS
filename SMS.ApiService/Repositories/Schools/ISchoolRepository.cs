using SMS.Common.Dtos.Schools;
using SMS.Common.Dtos;

namespace SMS.ApiService.Repositories.Schools
{
    public interface ISchoolRepository
    {
        Task<ApiResponse<IEnumerable<SchoolDto>>?> GetSchoolsAsync(CancellationToken cancellationToken);
        Task<ApiResponse<SchoolDto>?> GetSchoolByIdAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<SchoolDto>?> UpdateSchoolAsync(SchoolDto dto, CancellationToken cancellationToken);
        Task<ApiResponse<SchoolDto>?> DeleteSchoolAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<string>?> CreateOrUpdateSchoolsAsync(SchoolDto dto, CancellationToken cancellationToken);
    }
}
