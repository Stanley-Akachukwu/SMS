using Microsoft.EntityFrameworkCore;
using SMS.ApiService.Entities.Workflows;
using SMS.ApiService.Persistence;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Workflows;

namespace SMS.ApiService.Repositories.Workflows
{
    public class WorkflowRepository(AppDbContext _dbContext) : IWorkflowRepository
    {
        public async Task<ApiResponse<IEnumerable<WorkflowDto>>?> GetWorkflowsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var workflows = await _dbContext.Workflows
                    .Select(s => new WorkflowDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        Session = s.Session,
                        RegLink = s.RegLink,
                        CreatedByUserId = s.CreatedByUserId,
                        DateCreated = s.DateCreated,
                        WorkflowType = s.WorkflowType

                    })
                    .ToListAsync(cancellationToken);

                return ApiResponse<IEnumerable<WorkflowDto>>.Success(workflows, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<WorkflowDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<WorkflowDto>?> GetWorkflowByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = await _dbContext.Workflows.Where(s => s.Id == id)
                    .Select(s => new WorkflowDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Description = s.Description,
                        Session = s.Session,
                        RegLink = s.RegLink,
                        CreatedByUserId = s.CreatedByUserId,
                        DateCreated = s.DateCreated,
                        WorkflowType = s.WorkflowType

                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return ApiResponse<WorkflowDto>.Success(workflow, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<WorkflowDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<string>?> CreateOrUpdateWorkflowAsync(WorkflowDto dto, CancellationToken cancellationToken)
        {
            try
            {

                var workflow = await _dbContext.Workflows
                    .FirstOrDefaultAsync(s => s.Id == dto.Id, cancellationToken);

                string action = "created";
                int statusCode = StatusCodes.Status201Created;
                if (workflow == null)
                {
                    action = "created";
                    statusCode = StatusCodes.Status201Created;
                    workflow = new Workflow
                    {
                        Id = Ulid.NewUlid().ToString(),
                        Name = dto.Name,
                        Description = dto?.Description!,
                        IsActive = true,
                        DateCreated = DateTime.UtcNow,
                        CreatedByUserId = dto?.CreatedByUserId ?? string.Empty,
                        DateUpdated = DateTime.UtcNow,
                        UpdatedByUserId = dto.CreatedByUserId ?? string.Empty,
                        Session = dto.Session!,
                        RegLink = dto.RegLink!,
                        WorkflowType = dto.WorkflowType
                    };
                    _dbContext.Workflows.Add(workflow);
                }
                else
                {
                    action = "updated";
                    statusCode = StatusCodes.Status200OK;
                    workflow.Name = dto.Name;
                    workflow.Description = dto?.Description!;
                    workflow.Session = dto?.Session;
                    workflow.RegLink = dto?.RegLink!;
                    workflow.DateUpdated = DateTime.UtcNow;
                    workflow.UpdatedByUserId = dto?.CreatedByUserId ?? string.Empty;
                    workflow.WorkflowType = dto.WorkflowType;
                    _dbContext.Workflows.Update(workflow);
                }
                await _dbContext.SaveChangesAsync(cancellationToken);

                return ApiResponse<string>.Success($"Successfully {action}", statusCode);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApiResponse<string>?> DeleteWorkflowAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = await _dbContext.Workflows
                    .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

                if (workflow == null)
                    return ApiResponse<string>.Failure("Workflow not found.", StatusCodes.Status404NotFound);

                _dbContext.Workflows.Remove(workflow);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return ApiResponse<string>.Success("Deleted successfuly", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        
    }
}
