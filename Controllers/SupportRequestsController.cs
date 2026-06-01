using gov_API.Entities.Dtos.SupportRequests;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gov_API.Controllers
{
    [Route("api/support-requests")]
    [ApiController]
    [Authorize]
    public class SupportRequestsController : ControllerBase
    {
        private readonly ISupportRequestService _supportRequestService;

        public SupportRequestsController(ISupportRequestService supportRequestService)
        {
            _supportRequestService = supportRequestService;
        }

        [Authorize(Roles = "EntityAdmin,EntityUser")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateSupportRequestDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _supportRequestService.CreateAsync(dto, userId);
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
            var result = await _supportRequestService.GetAllAsync();
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

                var result = await _supportRequestService.GetMyAsync(userId);
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

                var result = await _supportRequestService.GetByIdAsync(id, userId, isPlatformAdmin);

                if (result == null)
                    return NotFound(new { message = "Support request not found." });

                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("{id:int}/reply")]
        public async Task<IActionResult> Reply(int id, CreateSupportReplyDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _supportRequestService.ReplyAsync(id, dto, userId);
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPut("{id:int}/close")]
        public async Task<IActionResult> Close(int id)
        {
            try
            {
                var result = await _supportRequestService.CloseAsync(id);
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
                var result = await _supportRequestService.DeleteAsync(id);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
