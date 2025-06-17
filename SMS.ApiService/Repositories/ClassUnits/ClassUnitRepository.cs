

using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Persistence;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Repositories.ClassUnits
{
    public class ClassUnitRepository(AppDbContext _dbContext) : IClassUnitRepository
    {
        public async Task<ApiResponse<IEnumerable<ClassUnitDto>>?> GetClassUnitsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var units = await _dbContext.ClassUnits
                    .Include(u => u.SchoolClass)
                    .Select(u => new ClassUnitDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        SchoolClassId = u.SchoolClassId,
                        SchoolClass = u.SchoolClass == null ? null : new SchoolClassDto
                        {
                            Id = u.SchoolClass.Id,
                            Name = u.SchoolClass.Name,
                            SchoolId = u.SchoolClass.SchoolId
                        }
                        // Add other properties as needed
                    })
                    .ToListAsync(cancellationToken);

                return ApiResponse<IEnumerable<ClassUnitDto>>.Success(units, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ClassUnitDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ClassUnitDto>?> GetClassUnitByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var unit = await _dbContext.ClassUnits
                    .Include(u => u.SchoolClass)
                    .Where(u => u.Id == id)
                    .Select(u => new ClassUnitDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        SchoolClassId = u.SchoolClassId,
                        SchoolClass = u.SchoolClass == null ? null : new SchoolClassDto
                        {
                            Id = u.SchoolClass.Id,
                            Name = u.SchoolClass.Name,
                            SchoolId = u.SchoolClass.SchoolId
                        }
                        // Add other properties as needed
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (unit == null)
                    return ApiResponse<ClassUnitDto>.Failure("Class unit not found.", StatusCodes.Status404NotFound);

                return ApiResponse<ClassUnitDto>.Success(unit, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClassUnitDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ClassUnitDto>?> UpdateClassUnitAsync(ClassUnitDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var unit = await _dbContext.ClassUnits
                    .FirstOrDefaultAsync(u => u.Id == dto.Id, cancellationToken);

                if (unit == null)
                    return ApiResponse<ClassUnitDto>.Failure("Class unit not found.", StatusCodes.Status404NotFound);

                unit.Name = dto.Name;
                unit.SchoolClassId = dto.SchoolClassId;
                // Update other properties as needed

                await _dbContext.SaveChangesAsync(cancellationToken);

                var updatedDto = new ClassUnitDto
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    SchoolClassId = unit.SchoolClassId
                    // Add other properties as needed
                };

                return ApiResponse<ClassUnitDto>.Success(updatedDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClassUnitDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<ClassUnitDto>?> DeleteClassUnitAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var unit = await _dbContext.ClassUnits
                    .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

                if (unit == null)
                    return ApiResponse<ClassUnitDto>.Failure("Class unit not found.", StatusCodes.Status404NotFound);

                _dbContext.ClassUnits.Remove(unit);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var deletedDto = new ClassUnitDto
                {
                    Id = unit.Id,
                    Name = unit.Name,
                    SchoolClassId = unit.SchoolClassId
                    // Add other properties as needed
                };

                return ApiResponse<ClassUnitDto>.Success(deletedDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClassUnitDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
