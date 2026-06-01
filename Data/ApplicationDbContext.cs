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
        public DbSet<AssessmentSubmission> AssessmentSubmissions { get; set; }
        public DbSet<AssessmentAnswer> AssessmentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<GovernmentEntity>()
                .HasMany(e => e.Users)
                .WithOne(u => u.GovernmentEntity)
                .HasForeignKey(u => u.GovernmentEntityId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<AssessmentSubmission>()
                .HasMany(s => s.Answers)
                .WithOne(a => a.AssessmentSubmission)
                .HasForeignKey(a => a.AssessmentSubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}