using System.Security.Claims;
using gov_API.Entities.Dtos.Policies;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gov_API.Controllers
{
    [Route("api/policies")]
    [ApiController]
    [Authorize]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PoliciesController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [Authorize(Roles = "PlatformAdmin,EntityAdmin,EntityUser")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _policyService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "PlatformAdmin,EntityAdmin,EntityUser")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _policyService.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Policy not found." });

            return Ok(result);
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePolicyDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _policyService.CreateAsync(dto, userId);

                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdatePolicyDto dto)
        {
            try
            {
                var result = await _policyService.UpdateAsync(id, dto);
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
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _policyService.DeleteAsync(id);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}