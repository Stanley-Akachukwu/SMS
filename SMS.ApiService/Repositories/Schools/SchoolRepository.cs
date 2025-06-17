using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Entities.Schools;
using SMS.ApiService.Persistence;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Schools;
using System;

namespace SMS.ApiService.Repositories.Schools
{
    public class SchoolRepository(AppDbContext _dbContext) : ISchoolRepository
    {
        public async Task<ApiResponse<IEnumerable<SchoolDto>>?> GetSchoolsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var schools = await _dbContext.Schools
                    .Include(s => s.Classes)
                    .Select(s => new SchoolDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        SettingId = s.SettingId,
                    })
                    .ToListAsync(cancellationToken);

                return ApiResponse<IEnumerable<SchoolDto>>.Success(schools, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SchoolDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SchoolDto>?> GetSchoolByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                var school = await _dbContext.Schools
                    .Include(s => s.Setting)
                    .Include(s => s.Classes)
                    .Where(s => s.Id == id)
                    .Select(s => new SchoolDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        SettingId = s.SettingId,
                        Setting = s.Setting == null ? null : new SchoolSettingDto
                        {
                            Id = s.Setting.Id,
                            Name = s.Setting.Name,
                            Description = s.Setting.Description,
                            ClassStartTime = s.Setting.ClassStartTime,
                            BreakStartTime = s.Setting.BreakStartTime,
                            BreakEndTime = s.Setting.BreakEndTime,
                            DismissalTime = s.Setting.DismissalTime,
                            MidTermBreak = s.Setting.MidTermBreak
                        },
                        Classes = s.Classes.Select(c => new SchoolClassDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            // Add other SchoolClassDto properties as needed
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);
#pragma warning restore CS8601 // Possible null reference assignment.

                if (school == null)
                    return ApiResponse<SchoolDto>.Failure("School not found.", StatusCodes.Status404NotFound);

                return ApiResponse<SchoolDto>.Success(school, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<SchoolDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ApiResponse<string>?> CreateOrUpdateSchoolsAsync(SchoolDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto?.Setting == null)
                    return ApiResponse<string>.Failure("Bad request.", StatusCodes.Status400BadRequest);

                var school = await _dbContext.Schools
                    .FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);

                string action = "created";
                int statusCode = StatusCodes.Status201Created;
                var setting = _dbContext.SchoolSettings.FirstOrDefault(s=>s.Id == dto.SettingId);
                if (school == null)
                {
                    action = "created";
                    statusCode = StatusCodes.Status201Created;
                    school = new School
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
                        Setting = setting!,
                    };
                    _dbContext.Schools.Add(school);
                }
                else
                {
                    action = "updated";
                    statusCode = StatusCodes.Status200OK;
                    school.Name = dto.Name;
                    school.Description = dto?.Description!;
                    school.SettingId = dto?.SettingId;
                    school.Setting = setting!;
                    school.DateUpdated = DateTime.UtcNow;
                    school.UpdatedByUserId = dto?.CreatedByUserId ?? string.Empty;
                    _dbContext.Schools.Update(school);
                }
                await _dbContext.SaveChangesAsync(cancellationToken);

                return ApiResponse<string>.Success($"Successfully {action}", statusCode);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SchoolDto>?> UpdateSchoolAsync(SchoolDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var school = await _dbContext.Schools
                    .FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);

                if (school == null || school.Setting == null)
                    return ApiResponse<SchoolDto>.Failure("School or setting not found.", StatusCodes.Status404NotFound);


                school.Name = dto.Name;
                school.Description = dto?.Description!;
                school.SettingId = dto?.SettingId;
                school.DateUpdated = DateTime.UtcNow;
                school.UpdatedByUserId = dto?.CreatedByUserId ?? string.Empty;
                await _dbContext.SaveChangesAsync(cancellationToken);

                return ApiResponse<SchoolDto>.Success(dto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<SchoolDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<SchoolDto>?> DeleteSchoolAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var school = await _dbContext.Schools
                    .Include(s => s.Classes)
                    .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

                if (school == null)
                    return ApiResponse<SchoolDto>.Failure("School not found.", StatusCodes.Status404NotFound);

                _dbContext.Schools.Remove(school);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var deletedDto = new SchoolDto
                {
                    Id = school.Id,
                    Name = school.Name,
                    Description = school.Description,
                    SettingId = school.SettingId,
                };

                return ApiResponse<SchoolDto>.Success(deletedDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<SchoolDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
