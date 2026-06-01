using gov_API.Enums;

namespace gov_API.Entities.Dtos.Notifications
{
    public class AdminNotificationDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public bool IsBroadcast { get; set; }

        public int RecipientsCount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
