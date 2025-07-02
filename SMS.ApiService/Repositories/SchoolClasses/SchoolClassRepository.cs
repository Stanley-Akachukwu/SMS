using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Entities.SchoolClasses;
using SMS.ApiService.Persistence;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Repositories.SchoolClasses
{
    public class SchoolClassRepository(AppDbContext _dbContext) : ISchoolClassRepository
    {
        public async Task<ApiResponse<IEnumerable<SchoolClassDto>>?> GetSchoolClassesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var classes = await _dbContext.SchoolClasses
                    .Include(sc => sc.School)
                    .Include(sc => sc.Setting)
                    .Select(sc => new SchoolClassDto
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        SchoolId = sc.SchoolId,
                        Description = sc.Description,
                        SettingId = sc.SettingId,
                        SchoolName = sc.School.Name,
                        //School = sc.School == null ? null : new SchoolDto
                        //{
                        //    Id = sc.School.Id,
                        //    Name = sc.School.Name,
                        //    Description = sc.School.Description,
                        //    SettingId = sc.School.SettingId
                        //},
                        //Setting = sc.Setting == null ? null : new ClassSettingDto
                        //{
                        //    Id = sc.Setting.Id,
                        //    Name = sc.Setting.Name,
                        //    Description = sc.Setting.Description
                        //},  
                    })
                    .ToListAsync(cancellationToken);

                return ApiResponse<IEnumerable<SchoolClassDto>>.Success(classes, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SchoolClassDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SchoolClassDto>?> GetSchoolClassByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var sc = await _dbContext.SchoolClasses
                    .Include(s => s.School)
                    .Include(s => s.Groups)
                    .Where(s => s.Id == id)
                    .Select(s => new SchoolClassDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        SchoolId = s.SchoolId,
                        School = s.School == null ? null : new SchoolDto
                        {
                            Id = s.School.Id,
                            Name = s.School.Name,
                            Description = s.School.Description,
                            SettingId = s.School.SettingId
                        },
                        Groups = s.Groups.Select(g => new ClassUnitDto
                        {
                            Id = g.Id,
                            Name = g.Name
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (sc == null)
                    return ApiResponse<SchoolClassDto>.Failure("School class not found.", StatusCodes.Status404NotFound);

                return ApiResponse<SchoolClassDto>.Success(sc, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<SchoolClassDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ApiResponse<string>?> CreateOrUpdateClassAsync(SchoolClassDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto?.School == null)
                    return ApiResponse<string>.Failure("Bad request.", StatusCodes.Status400BadRequest);

                var schoolClass = await _dbContext.SchoolClasses
                    .FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);

                string action = "created";
                int statusCode = StatusCodes.Status201Created;
                var setting = _dbContext.ClassSettings.FirstOrDefault(s => s.Id == dto.SettingId);
                if (schoolClass == null)
                {
                    action = "created";
                    statusCode = StatusCodes.Status201Created;
                    schoolClass = new SchoolClass
                    {
                        Id = Ulid.NewUlid().ToString(),
                        Name = dto.Name,
                        Description = dto?.Description!,
                        SettingId = dto?.SettingId,
                        IsActive = true,
                        DateCreated = DateTime.UtcNow,
                        CreatedByUserId = dto?.CreatedByUserId ?? string.Empty,
                        DateUpdated = DateTime.UtcNow,
                        UpdatedByUserId = dto.CreatedByUserId ?? string.Empty,
                        SchoolId = dto.SchoolId,
                        Setting = setting!,
                    };
                    _dbContext.SchoolClasses.Add(schoolClass);
                }
                else
                {
                    action = "updated";
                    statusCode = StatusCodes.Status200OK;
                    schoolClass.Name = dto.Name;
                    schoolClass.Description = dto?.Description!;
                    schoolClass.SettingId = dto?.SettingId;
                    schoolClass.SchoolId = dto.SchoolId;
                    schoolClass.Setting = setting!;
                    schoolClass.DateUpdated = DateTime.UtcNow;
                    schoolClass.UpdatedByUserId = dto?.CreatedByUserId ?? string.Empty;
                    _dbContext.SchoolClasses.Update(schoolClass);
                }
                await _dbContext.SaveChangesAsync(cancellationToken);

                return ApiResponse<string>.Success($"Successfully {action}", statusCode);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ApiResponse<SchoolClassDto>?> UpdateSchoolClassAsync(SchoolClassDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var sc = await _dbContext.SchoolClasses
                    .Include(s => s.Groups)
                    .FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);

                if (sc == null)
                    return ApiResponse<SchoolClassDto>.Failure("School class not found.", StatusCodes.Status404NotFound);

                sc.Name = dto.Name;
                sc.SchoolId = dto.SchoolId;

                // Optionally update groups here if needed

                await _dbContext.SaveChangesAsync(cancellationToken);

                var updatedDto = new SchoolClassDto
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    SchoolId = sc.SchoolId,
                    Groups = sc.Groups.Select(g => new ClassUnitDto
                    {
                        Id = g.Id,
                        Name = g.Name
                    }).ToList()
                };

                return ApiResponse<SchoolClassDto>.Success(updatedDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<SchoolClassDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SchoolClassDto>?> DeleteSchoolClassAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var sc = await _dbContext.SchoolClasses
                    .Include(s => s.Groups)
                    .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

                if (sc == null)
                    return ApiResponse<SchoolClassDto>.Failure("School class not found.", StatusCodes.Status404NotFound);

                _dbContext.SchoolClasses.Remove(sc);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var deletedDto = new SchoolClassDto
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    SchoolId = sc.SchoolId
                };

                return ApiResponse<SchoolClassDto>.Success(deletedDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<SchoolClassDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        
    }
}
