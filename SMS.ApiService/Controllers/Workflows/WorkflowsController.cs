using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.Workflows;
using SMS.Common.Dtos.Workflows;

namespace SMS.ApiService.Controllers.Workflows
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class WorkflowsController(IWorkflowRepository _repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetWorkflowsAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetWorkflowsAsync(cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdateWorkflowsAsync([FromBody] WorkflowDto dto, CancellationToken cancellationToken)
        {
            var result = await _repository.CreateOrUpdateWorkflowAsync(dto, cancellationToken);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetWorkflowByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.GetWorkflowByIdAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWorkflowAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteWorkflowAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
