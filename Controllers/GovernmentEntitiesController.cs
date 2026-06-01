using gov_API.DTOs.GovernmentEntities;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gov_API.Controllers
{
    [Route("api/government-entities")]
    [ApiController]
    public class GovernmentEntitiesController : ControllerBase
    {
        private readonly IGovernmentEntityService _governmentEntityService;

        public GovernmentEntitiesController(IGovernmentEntityService governmentEntityService)
        {
            _governmentEntityService = governmentEntityService;
        }

        [AllowAnonymous]
        [HttpPost("join-request")]
        public async Task<IActionResult> SubmitJoinRequest(GovernmentEntityJoinRequestDto dto)
        {
            try
            {
                var result = await _governmentEntityService.SubmitJoinRequestAsync(dto);
                return Ok(new { message = result });
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
            var result = await _governmentEntityService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _governmentEntityService.GetPendingAsync();
            return Ok(result);
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _governmentEntityService.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Government entity not found." });

            return Ok(result);
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPut("{id:int}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var result = await _governmentEntityService.ApproveAsync(id);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPut("{id:int}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                var result = await _governmentEntityService.RejectAsync(id);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPut("{id:int}/suspend")]
        public async Task<IActionResult> Suspend(int id)
        {
            try
            {
                var result = await _governmentEntityService.SuspendAsync(id);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpGet("pending/count")]
        public async Task<IActionResult> GetPendingCount()
        {
            var count = await _governmentEntityService.GetPendingCountAsync();
            return Ok(new { count });
        }
    }
}