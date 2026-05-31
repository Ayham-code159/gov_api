namespace gov_API.DTOs.GovernmentEntities
{
    public class ReadinessAnswerDto
    {
        public int QuestionId { get; set; }

        public int Score { get; set; }

        public string? Note { get; set; }
    }
}