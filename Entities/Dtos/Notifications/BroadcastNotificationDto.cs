using gov_API.Enums;

namespace gov_API.Entities.Dtos.Notifications
{
    public class BroadcastNotificationDto
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }
    }

}
