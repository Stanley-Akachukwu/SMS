using SMS.Common.Dtos.Schools;
using SMS.Common.Dtos;

namespace SMS.ApiService.Repositories.ClassUnits
{
    public interface IClassUnitRepository
    {
        Task<ApiResponse<IEnumerable<ClassUnitDto>>?> GetClassUnitsAsync(CancellationToken cancellationToken);
        Task<ApiResponse<ClassUnitDto>?> GetClassUnitByIdAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<ClassUnitDto>?> UpdateClassUnitAsync(ClassUnitDto dto, CancellationToken cancellationToken);
        Task<ApiResponse<ClassUnitDto>?> DeleteClassUnitAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<string>?> CreateOrUpdateClassUnitAsync(ClassUnitDto dto, CancellationToken cancellationToken);

    }
}
