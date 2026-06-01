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
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationRecipient> NotificationRecipients { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<SupportReply> SupportReplies { get; set; }

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

            builder.Entity<Notification>()
                .HasMany(n => n.Recipients)
                .WithOne(r => r.Notification)
                .HasForeignKey(r => r.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NotificationRecipient>()
                .HasOne(r => r.GovernmentEntity)
                .WithMany()
                .HasForeignKey(r => r.GovernmentEntityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SupportRequest>()
                .HasMany(r => r.Replies)
                .WithOne(r => r.SupportRequest)
                .HasForeignKey(r => r.SupportRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SupportRequest>()
                .HasOne(r => r.GovernmentEntity)
                .WithMany()
                .HasForeignKey(r => r.GovernmentEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}