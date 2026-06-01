namespace gov_API.Entities.Dtos.SupportRequests
{
    public class SupportReplyDto
    {
        public int Id { get; set; }

        public int SupportRequestId { get; set; }

        public string SenderUserId { get; set; } = string.Empty;

        public string SenderFullName { get; set; } = string.Empty;

        public string SenderEmail { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }

}
