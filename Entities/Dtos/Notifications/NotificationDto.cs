using gov_API.Enums;

namespace gov_API.Entities.Dtos.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public int NotificationRecipientId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public bool IsBroadcast { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ReadAt { get; set; }
    }

}
