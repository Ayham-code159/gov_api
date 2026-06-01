namespace gov_API.Entities.Dtos.Maturity
{
    public class MaturityAnswerResultDto
    {
        public string QuestionKey { get; set; } = string.Empty;

        public int Score { get; set; }

        public string? Note { get; set; }
    }

}
