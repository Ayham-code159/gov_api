using gov_API.Enums;

namespace gov_API.Entities.Dtos.Reports
{
    public class AssessmentReportItemDto
    {
        public int SubmissionId { get; set; }

        public int GovernmentEntityId { get; set; }

        public string GovernmentEntityName { get; set; } = string.Empty;

        public AssessmentType AssessmentType { get; set; }

        public double FinalScore { get; set; }

        public double Percentage { get; set; }

        public AssessmentResultLevel ResultLevel { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
