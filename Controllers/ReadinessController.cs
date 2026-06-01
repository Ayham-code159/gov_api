using gov_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gov_API.Controllers
{
    [Route("api/readiness")]
    [ApiController]
    [Authorize(Roles = "PlatformAdmin")]
    public class ReadinessController : ControllerBase
    {
        private readonly IReadinessService _readinessService;

        public ReadinessController(IReadinessService readinessService)
        {
            _readinessService = readinessService;
        }

        [HttpGet("results")]
        public async Task<IActionResult> GetAllResults()
        {
            var result = await _readinessService.GetAllResultsAsync();
            return Ok(result);
        }

        [HttpGet("results/{entityId:int}")]
        public async Task<IActionResult> GetResultByEntityId(int entityId)
        {
            var result = await _readinessService.GetResultByEntityIdAsync(entityId);

            if (result == null)
                return NotFound(new { message = "Readiness result not found." });

            return Ok(result);
        }
    }
}