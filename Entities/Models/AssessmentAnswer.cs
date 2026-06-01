namespace gov_API.Entities.Models
{
    public class AssessmentAnswer
    {
        public int Id { get; set; }

        public int AssessmentSubmissionId { get; set; }
        public AssessmentSubmission AssessmentSubmission { get; set; } = null!;

        public string QuestionKey { get; set; } = string.Empty;

        public int Score { get; set; }

        public string? Note { get; set; }
    }
}