using gov_API.Data;
using gov_API.Entities.Dtos.Notifications;
using gov_API.Entities.Models;
using gov_API.Enums;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace gov_API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotificationService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> SendToEntityAsync(SendNotificationToEntityDto dto, string adminUserId)
        {
            var entity = await _context.GovernmentEntities
                .FirstOrDefaultAsync(e => e.Id == dto.GovernmentEntityId);

            if (entity == null)
                throw new KeyNotFoundException("Government entity not found.");

            if (entity.Status != EntityStatus.Approved)
                throw new InvalidOperationException("You can only send notifications to approved entities.");

            var notification = new Notification
            {
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                IsBroadcast = false,
                CreatedByUserId = adminUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            var recipient = new NotificationRecipient
            {
                NotificationId = notification.Id,
                GovernmentEntityId = entity.Id,
                IsRead = false
            };

            await _context.NotificationRecipients.AddAsync(recipient);
            await _context.SaveChangesAsync();

            return "Notification sent successfully.";
        }

        public async Task<string> BroadcastAsync(BroadcastNotificationDto dto, string adminUserId)
        {
            var approvedEntities = await _context.GovernmentEntities
                .Where(e => e.Status == EntityStatus.Approved)
                .ToListAsync();

            if (!approvedEntities.Any())
                throw new InvalidOperationException("There are no approved entities to notify.");

            var notification = new Notification
            {
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                IsBroadcast = true,
                CreatedByUserId = adminUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();

            var recipients = approvedEntities.Select(e => new NotificationRecipient
            {
                NotificationId = notification.Id,
                GovernmentEntityId = e.Id,
                IsRead = false
            }).ToList();

            await _context.NotificationRecipients.AddRangeAsync(recipients);
            await _context.SaveChangesAsync();

            return "Broadcast notification sent successfully.";
        }

        public async Task<IEnumerable<NotificationDto>> GetMyNotificationsAsync(string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.GovernmentEntityId == null)
                throw new InvalidOperationException("User is not linked to a government entity.");

            return await _context.NotificationRecipients
                .Where(r => r.GovernmentEntityId == user.GovernmentEntityId.Value)
                .Include(r => r.Notification)
                .OrderByDescending(r => r.Notification.CreatedAt)
                .Select(r => new NotificationDto
                {
                    Id = r.Notification.Id,
                    NotificationRecipientId = r.Id,
                    Title = r.Notification.Title,
                    Message = r.Notification.Message,
                    Type = r.Notification.Type,
                    IsBroadcast = r.Notification.IsBroadcast,
                    IsRead = r.IsRead,
                    CreatedAt = r.Notification.CreatedAt,
                    ReadAt = r.ReadAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminNotificationDto>> GetAllAsync()
        {
            return await _context.Notifications
                .Include(n => n.Recipients)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new AdminNotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type,
                    IsBroadcast = n.IsBroadcast,
                    RecipientsCount = n.Recipients.Count,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<string> MarkAsReadAsync(int recipientId, string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.GovernmentEntityId == null)
                throw new InvalidOperationException("User is not linked to a government entity.");

            var recipient = await _context.NotificationRecipients
                .FirstOrDefaultAsync(r =>
                    r.Id == recipientId &&
                    r.GovernmentEntityId == user.GovernmentEntityId.Value);

            if (recipient == null)
                throw new KeyNotFoundException("Notification not found.");

            recipient.IsRead = true;
            recipient.ReadAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return "Notification marked as read.";
        }

        public async Task<string> DeleteAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                throw new KeyNotFoundException("Notification not found.");

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return "Notification deleted successfully.";
        }
    }

}
