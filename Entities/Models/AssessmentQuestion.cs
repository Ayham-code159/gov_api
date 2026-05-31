using gov_API.Enums;

namespace gov_API.Entities.Models
{
    public class AssessmentQuestion
    {
        public int Id { get; set; }

        public int AssessmentAxisId { get; set; }
        public AssessmentAxis AssessmentAxis { get; set; } = null!;

        public string QuestionText { get; set; } = string.Empty;

        public AssessmentType AssessmentType { get; set; }

        public int OrderNumber { get; set; }
    }
}