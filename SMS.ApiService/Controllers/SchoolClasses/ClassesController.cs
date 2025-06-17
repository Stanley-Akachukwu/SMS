using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.SchoolClasses;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Controllers.SchoolClasses
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController(ISchoolClassRepository _repository) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult> GetSchoolClassesAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetSchoolClassesAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSchoolClassByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.GetSchoolClassByIdAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSchoolClassAsync(string id, [FromBody] SchoolClassDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                return BadRequest("ID in URL and body do not match.");

            var result = await _repository.UpdateSchoolClassAsync(dto, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSchoolClassAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteSchoolClassAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
