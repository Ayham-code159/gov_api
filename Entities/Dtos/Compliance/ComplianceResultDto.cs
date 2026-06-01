using gov_API.Enums;

namespace gov_API.Entities.Dtos.Compliance
{
    public class ComplianceResultDto
    {
        public int SubmissionId { get; set; }

        public int GovernmentEntityId { get; set; }

        public string GovernmentEntityName { get; set; } = string.Empty;

        public double FinalScore { get; set; }

        public double CompliancePercentage { get; set; }

        public AssessmentResultLevel ResultLevel { get; set; }

        public DateTime SubmittedAt { get; set; }

        public List<ComplianceAnswerResultDto> Answers { get; set; } = new();
    }

}
