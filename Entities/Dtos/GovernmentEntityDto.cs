using gov_API.Enums;

namespace gov_API.DTOs.GovernmentEntities
{
    public class GovernmentEntityDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public EntityStatus Status { get; set; }

        public double ReadinessScore { get; set; }

        public double CompliancePercentage { get; set; }

        public double MaturityScore { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }
    }
}