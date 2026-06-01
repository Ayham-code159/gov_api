using gov_API.Enums;

namespace gov_API.DTOs.Readiness
{
    public class ReadinessResultDto
    {
        public int SubmissionId { get; set; }

        public int GovernmentEntityId { get; set; }

        public string GovernmentEntityName { get; set; } = string.Empty;

        public double FinalScore { get; set; }

        public AssessmentResultLevel ResultLevel { get; set; }

        public DateTime SubmittedAt { get; set; }

        public List<ReadinessAnswerResultDto> Answers { get; set; } = new();
    }
}