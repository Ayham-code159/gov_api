namespace gov_API.Entities.Dtos.Policies
{
    public class PolicyDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string CreatedByUserId { get; set; } = string.Empty;

        public string CreatedByFullName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

}
