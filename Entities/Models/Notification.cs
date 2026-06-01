using gov_API.Enums;

namespace gov_API.Entities.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public bool IsBroadcast { get; set; }

        public string CreatedByUserId { get; set; } = string.Empty;
        public ApplicationUser CreatedByUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<NotificationRecipient> Recipients { get; set; } = new List<NotificationRecipient>();
    }

}
