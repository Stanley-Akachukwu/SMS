using SMS.Common.Dtos;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Repositories.SchoolClasses
{
    public interface ISchoolClassRepository
    {
        Task<ApiResponse<IEnumerable<SchoolClassDto>>?> GetSchoolClassesAsync(CancellationToken cancellationToken);
        Task<ApiResponse<SchoolClassDto>?> GetSchoolClassByIdAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<SchoolClassDto>?> UpdateSchoolClassAsync(SchoolClassDto dto, CancellationToken cancellationToken);
        Task<ApiResponse<SchoolClassDto>?> DeleteSchoolClassAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<string>?> CreateOrUpdateClassAsync(SchoolClassDto dto, CancellationToken cancellationToken);
    }
}
