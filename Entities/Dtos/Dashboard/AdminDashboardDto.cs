namespace gov_API.Entities.Dtos.Dashboard
{
    public class AdminDashboardDto
    {
        public int TotalGovernmentEntities { get; set; }

        public int PendingGovernmentEntities { get; set; }

        public int ApprovedGovernmentEntities { get; set; }

        public int RejectedGovernmentEntities { get; set; }

        public int SuspendedGovernmentEntities { get; set; }

        public double AverageReadinessScore { get; set; }

        public int TotalJoinRequests { get; set; }
    }
}
