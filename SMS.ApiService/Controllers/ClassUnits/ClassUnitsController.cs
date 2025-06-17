using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.ClassUnits;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Controllers.ClassUnits
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassUnitsController(IClassUnitRepository _repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetClassUnitsAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetClassUnitsAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetClassUnitByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.GetClassUnitByIdAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClassUnitAsync(string id, [FromBody] ClassUnitDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                return BadRequest("ID in URL and body do not match.");

            var result = await _repository.UpdateClassUnitAsync(dto, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteClassUnitAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteClassUnitAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
