using gov_API.Enums;

namespace gov_API.Entities.Models
{
    public class SupportRequest
    {
        public int Id { get; set; }

        public int GovernmentEntityId { get; set; }
        public GovernmentEntity GovernmentEntity { get; set; } = null!;

        public string CreatedByUserId { get; set; } = string.Empty;
        public ApplicationUser CreatedByUser { get; set; } = null!;

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public SupportRequestStatus Status { get; set; } = SupportRequestStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ClosedAt { get; set; }

        public ICollection<SupportReply> Replies { get; set; } = new List<SupportReply>();
    }
}