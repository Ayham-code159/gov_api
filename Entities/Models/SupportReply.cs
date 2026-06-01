namespace gov_API.Entities.Models
{
    public class SupportReply
    {
        public int Id { get; set; }

        public int SupportRequestId { get; set; }
        public SupportRequest SupportRequest { get; set; } = null!;

        public string SenderUserId { get; set; } = string.Empty;
        public ApplicationUser SenderUser { get; set; } = null!;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}