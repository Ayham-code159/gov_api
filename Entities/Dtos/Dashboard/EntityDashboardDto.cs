using gov_API.Enums;

namespace gov_API.Entities.Dtos.Dashboard
{
    public class EntityDashboardDto
    {
        public int GovernmentEntityId { get; set; }

        public string GovernmentEntityName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public EntityStatus Status { get; set; }

        public double ReadinessScore { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }
    }

}
