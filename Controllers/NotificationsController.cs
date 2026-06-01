using gov_API.Entities.Dtos.Notifications;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gov_API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _notificationService.GetAllAsync();
            return Ok(result);
        }

        [Authorize(Roles = "EntityAdmin,EntityUser")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _notificationService.GetMyNotificationsAsync(userId);
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
        [HttpPost("send-to-entity")]
        public async Task<IActionResult> SendToEntity(SendNotificationToEntityDto dto)
        {
            try
            {
                var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(adminUserId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _notificationService.SendToEntityAsync(dto, adminUserId);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PlatformAdmin")]
        [HttpPost("broadcast")]
        public async Task<IActionResult> Broadcast(BroadcastNotificationDto dto)
        {
            try
            {
                var adminUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(adminUserId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _notificationService.BroadcastAsync(dto, adminUserId);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "EntityAdmin,EntityUser")]
        [HttpPut("{recipientId:int}/read")]
        public async Task<IActionResult> MarkAsRead(int recipientId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId))
                    return Unauthorized(new { message = "Invalid token." });

                var result = await _notificationService.MarkAsReadAsync(recipientId, userId);
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
        [HttpDelete("{notificationId:int}")]
        public async Task<IActionResult> Delete(int notificationId)
        {
            try
            {
                var result = await _notificationService.DeleteAsync(notificationId);
                return Ok(new { message = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
