using gov_API.Enums;

namespace gov_API.Entities.Dtos.SupportRequests
{
    public class SupportRequestDto
    {
        public int Id { get; set; }

        public int GovernmentEntityId { get; set; }

        public string GovernmentEntityName { get; set; } = string.Empty;

        public string CreatedByUserId { get; set; } = string.Empty;

        public string CreatedByFullName { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public SupportRequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ClosedAt { get; set; }

        public List<SupportReplyDto> Replies { get; set; } = new();
    }

}
