using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.Settings;
using SMS.Common.Dtos.Schools;

namespace SMS.ApiService.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSettingsController(IClassSettingsRepository _repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetClassSettingsAsync(CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetClassSettingsAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetClassSettingByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.GetClassSettingByIdAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClassSettingAsync(string id, [FromBody] ClassSettingDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                return BadRequest("ID in URL and body do not match.");

            var result = await _repository.UpdateClassSettingAsync(dto, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
