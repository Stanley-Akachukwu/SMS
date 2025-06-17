using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.Settings;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolSettingsController(ISchoolSettingsRepository _repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetSchoolSettingsAsync(CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetSchoolSettingsAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSchoolSettingByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.GetSchoolSettingByIdAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSchoolSettingAsync(string id, [FromBody] SchoolSettingDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                return BadRequest("ID in URL and body do not match.");

            var result = await _repository.UpdateSchoolSettingAsync(dto, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
