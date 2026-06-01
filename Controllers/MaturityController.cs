using System.Security.Claims;
using gov_API.Entities.Dtos.Maturity;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gov_API.Controllers
{
    [Route("api/maturity")]
    [ApiController]
    [Authorize]
    public class MaturityController : ControllerBase
    {
        private readonly IMaturityService _maturityService;

        public MaturityController(IMaturityService maturityService)
        {
            _maturityService = maturityService;
        }

        [Authorize(Roles = "EntityAdmin,EntityUser")]
        [HttpPost("submit")]
        public async Task<IActionResult> Submit(SubmitMaturityAssessmentDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _maturityService.SubmitAsync(dto, userId);
                return Ok(new { message = result });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "EntityAdmin,EntityUser")]
        [HttpGet("my-result")]
        public async Task<IActionResult> GetMyResult()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _maturityService.GetMyResultAsync(userId);

                if (result == null)
                    return NotFound(new { message = "Maturity result not found." });

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpGet("results")]
        public async Task<IActionResult> GetAllResults()
        {
            var result = await _maturityService.GetAllResultsAsync();
            return Ok(result);
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpGet("results/{entityId:int}")]
        public async Task<IActionResult> GetResultByEntityId(int entityId)
        {
            var result = await _maturityService.GetResultByEntityIdAsync(entityId);

            if (result == null)
                return NotFound(new { message = "Maturity result not found." });

            return Ok(result);
        }
    }
}