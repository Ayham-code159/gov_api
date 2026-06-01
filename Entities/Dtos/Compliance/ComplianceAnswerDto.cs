namespace gov_API.Entities.Dtos.Compliance
{
    public class ComplianceAnswerDto
    {
        public string QuestionKey { get; set; } = string.Empty;

        public int Score { get; set; }

        public string? Note { get; set; }
    }

}
