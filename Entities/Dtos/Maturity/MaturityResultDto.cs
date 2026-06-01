using gov_API.Enums;

namespace gov_API.Entities.Dtos.Maturity
{
    public class MaturityResultDto
    {
        public int SubmissionId { get; set; }

        public int GovernmentEntityId { get; set; }

        public string GovernmentEntityName { get; set; } = string.Empty;

        public double FinalScore { get; set; }

        public AssessmentResultLevel ResultLevel { get; set; }

        public DateTime SubmittedAt { get; set; }

        public List<MaturityAnswerResultDto> Answers { get; set; } = new();
    }

}
