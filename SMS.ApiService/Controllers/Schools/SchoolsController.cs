using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.Schools;
using SMS.Common.Dtos.Schools;
using SMS.Common.Dtos.Users;

namespace SMS.ApiService.Controllers.Schools
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController(ISchoolRepository _repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetSchoolsAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetSchoolsAsync(cancellationToken);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> CreateOrUpdateSchoolsAsync([FromBody] SchoolDto dto, CancellationToken cancellationToken)
        {
            var result = await _repository.CreateOrUpdateSchoolsAsync(dto,cancellationToken);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSchoolByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.GetSchoolByIdAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSchoolAsync(string id, [FromBody] SchoolDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                return BadRequest("ID in URL and body do not match.");

            var result = await _repository.UpdateSchoolAsync(dto, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSchoolAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteSchoolAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
