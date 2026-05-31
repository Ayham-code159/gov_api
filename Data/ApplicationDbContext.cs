using gov_API.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace gov_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GovernmentEntity> GovernmentEntities { get; set; }
        public DbSet<AssessmentAxis> AssessmentAxes { get; set; }
        public DbSet<AssessmentQuestion> AssessmentQuestions { get; set; }
        public DbSet<AssessmentSubmission> AssessmentSubmissions { get; set; }
        public DbSet<AssessmentAnswer> AssessmentAnswers { get; set; }
    }
}