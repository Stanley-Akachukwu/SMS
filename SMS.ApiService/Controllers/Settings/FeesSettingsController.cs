using Microsoft.AspNetCore.Mvc;
using SMS.ApiService.Repositories.Settings;
using SMS.Common.Dtos.Fees;

namespace SMS.ApiService.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeesSettingsController(IFeesSettingRepository _repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetFeesSettingsAsync(CancellationToken cancellationToken)
        {
            return Ok(await _repository.GetFeesSettingsAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetFeesSettingByIdAsync(string id, CancellationToken cancellationToken)
        {
            var result = await _repository.GetFeesSettingByIdAsync(id, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFeesSettingAsync(string id, [FromBody] FeesSettingDto dto, CancellationToken cancellationToken)
        {
            if (dto.Id != id)
                return BadRequest("ID in URL and body do not match.");

            var result = await _repository.UpdateFeesSettingAsync(dto, cancellationToken);
            if (result == null || !result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
