using gov_API.Enums;

namespace gov_API.Entities.Models
{
    public class GovernmentEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public EntityStatus Status { get; set; } = EntityStatus.Pending;

        public double ReadinessScore { get; set; }

        public double MaturityScore { get; set; }

        public double ComplianceScore { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ApprovedAt { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

        public ICollection<AssessmentSubmission> AssessmentSubmissions { get; set; } = new List<AssessmentSubmission>();
    }
}