namespace gov_API.Entities.Dtos.Maturity
{
    public class SubmitMaturityAssessmentDto
    {
        public List<MaturityAnswerDto> Answers { get; set; } = new();
    }

}
