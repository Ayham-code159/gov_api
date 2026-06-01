using System.Security.Claims;
using gov_API.Entities.Dtos.Vulnerabilities;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gov_API.Controllers
{
    [Route("api/vulnerabilities")]
    [ApiController]
    [Authorize]
    public class VulnerabilitiesController : ControllerBase
    {
        private readonly IVulnerabilityService _vulnerabilityService;

        public VulnerabilitiesController(IVulnerabilityService vulnerabilityService)
        {
            _vulnerabilityService = vulnerabilityService;
        }

        [Authorize(Roles = "EntityAdmin,EntityUser")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateVulnerabilityDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _vulnerabilityService.CreateAsync(dto, userId);
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

        [Authorize(Roles = "PlatformAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vulnerabilityService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "EntityAdmin,EntityUser")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _vulnerabilityService.GetMyAsync(userId);
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var isPlatformAdmin = User.IsInRole("PlatformAdmin");

                var result = await _vulnerabilityService.GetByIdAsync(id, userId, isPlatformAdmin);

                if (result == null)
                    return NotFound(new { message = "Vulnerability not found." });

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateVulnerabilityDto dto)
        {
            try
            {
                var result = await _vulnerabilityService.UpdateAsync(id, dto);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateVulnerabilityStatusDto dto)
        {
            try
            {
                var result = await _vulnerabilityService.UpdateStatusAsync(id, dto);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _vulnerabilityService.DeleteAsync(id);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}