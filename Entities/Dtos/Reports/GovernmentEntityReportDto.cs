using gov_API.Enums;

namespace gov_API.Entities.Dtos.Reports
{
    public class GovernmentEntityReportDto
    {
        public int GovernmentEntityId { get; set; }

        public string GovernmentEntityName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public EntityStatus Status { get; set; }

        public double ReadinessScore { get; set; }

        public double MaturityScore { get; set; }

        public double ComplianceScore { get; set; }

        public int TotalVulnerabilities { get; set; }

        public int OpenVulnerabilities { get; set; }

        public int InProgressVulnerabilities { get; set; }

        public int ResolvedVulnerabilities { get; set; }

        public int ClosedVulnerabilities { get; set; }

        public int CriticalVulnerabilities { get; set; }

        public int HighVulnerabilities { get; set; }

        public int TotalSupportRequests { get; set; }

        public int OpenSupportRequests { get; set; }

        public int InProgressSupportRequests { get; set; }

        public int ClosedSupportRequests { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }
    }

}
