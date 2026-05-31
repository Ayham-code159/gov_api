using gov_API.Enums;

namespace gov_API.Entities.Models
{
    public class AssessmentSubmission
    {
        public int Id { get; set; }

        public int GovernmentEntityId { get; set; }
        public GovernmentEntity GovernmentEntity { get; set; } = null!;

        public AssessmentType AssessmentType { get; set; }

        public double FinalScore { get; set; }

        public AssessmentResultLevel ResultLevel { get; set; }

        public string SubmittedByUserId { get; set; } = string.Empty;
        public ApplicationUser SubmittedByUser { get; set; } = null!;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public ICollection<AssessmentAnswer> Answers { get; set; } = new List<AssessmentAnswer>();
    }
}