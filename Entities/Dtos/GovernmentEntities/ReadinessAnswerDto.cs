namespace gov_API.DTOs.GovernmentEntities
{
    public class ReadinessAnswerDto
    {
        public string QuestionKey { get; set; } = string.Empty;

        public int Score { get; set; }

        public string? Note { get; set; }
    }
}