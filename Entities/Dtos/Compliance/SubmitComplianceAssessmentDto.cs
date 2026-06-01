namespace gov_API.Entities.Dtos.Compliance
{
    public class SubmitComplianceAssessmentDto
    {
        public List<ComplianceAnswerDto> Answers { get; set; } = new();
    }

}
