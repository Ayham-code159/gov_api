namespace gov_API.DTOs.Readiness
{
    public class ReadinessAnswerResultDto
    {
        public string QuestionKey { get; set; } = string.Empty;

        public int Score { get; set; }

        public string? Note { get; set; }
    }
}