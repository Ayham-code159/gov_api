using gov_API.Enums;

namespace gov_API.Entities.Models
{
    public class AssessmentAxis
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public AssessmentType AssessmentType { get; set; }

        public int OrderNumber { get; set; }

        public ICollection<AssessmentQuestion> Questions { get; set; } = new List<AssessmentQuestion>();
    }
}