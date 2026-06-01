namespace gov_API.Entities.Models
{
    public class NotificationRecipient
    {
        public int Id { get; set; }

        public int NotificationId { get; set; }
        public Notification Notification { get; set; } = null!;

        public int GovernmentEntityId { get; set; }
        public GovernmentEntity GovernmentEntity { get; set; } = null!;

        public bool IsRead { get; set; }

        public DateTime? ReadAt { get; set; }
    }

}
