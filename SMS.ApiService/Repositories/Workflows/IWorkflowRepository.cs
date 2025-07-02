using SMS.Common.Dtos;
using SMS.Common.Dtos.Workflows;

namespace SMS.ApiService.Repositories.Workflows
{
    public interface IWorkflowRepository
    {
        Task<ApiResponse<IEnumerable<WorkflowDto>>?> GetWorkflowsAsync(CancellationToken cancellationToken);
        Task<ApiResponse<WorkflowDto>?> GetWorkflowByIdAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<string>?> DeleteWorkflowAsync(string id, CancellationToken cancellationToken);
        Task<ApiResponse<string>?> CreateOrUpdateWorkflowAsync(WorkflowDto dto, CancellationToken cancellationToken);
    }
}
