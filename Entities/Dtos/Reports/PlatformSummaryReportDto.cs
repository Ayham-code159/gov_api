namespace gov_API.Entities.Dtos.Reports
{
    public class PlatformSummaryReportDto
    {
        public int TotalEntities { get; set; }

        public int PendingEntities { get; set; }

        public int ApprovedEntities { get; set; }

        public int RejectedEntities { get; set; }

        public int SuspendedEntities { get; set; }

        public double AverageReadinessScore { get; set; }

        public double AverageMaturityScore { get; set; }

        public double AverageComplianceScore { get; set; }

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
    }

}
