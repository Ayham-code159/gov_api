using gov_API.Entities.Dtos.Notifications;

namespace gov_API.Interfaces
{
    public interface INotificationService
    {
        Task<string> SendToEntityAsync(SendNotificationToEntityDto dto, string adminUserId);

        Task<string> BroadcastAsync(BroadcastNotificationDto dto, string adminUserId);

        Task<IEnumerable<NotificationDto>> GetMyNotificationsAsync(string userId);

        Task<IEnumerable<AdminNotificationDto>> GetAllAsync();

        Task<string> MarkAsReadAsync(int recipientId, string userId);

        Task<string> DeleteAsync(int notificationId);
    }

}
