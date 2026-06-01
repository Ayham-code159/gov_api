using gov_API.Data;
using gov_API.Entities.Dtos.Compliance;
using gov_API.Entities.Models;
using gov_API.Enums;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gov_API.Services
{
    public class ComplianceService : IComplianceService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ComplianceService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> SubmitAsync(SubmitComplianceAssessmentDto dto, string userId)
        {
            if (!dto.Answers.Any())
                throw new InvalidOperationException("Compliance answers are required.");

            foreach (var answer in dto.Answers)
            {
                if (string.IsNullOrWhiteSpace(answer.QuestionKey))
                    throw new InvalidOperationException("Question key is required.");

                if (answer.Score < 1 || answer.Score > 5)
                    throw new InvalidOperationException("Score must be between 1 and 5.");
            }

            var user = await _userManager.Users
                .Include(u => u.GovernmentEntity)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.GovernmentEntityId == null || user.GovernmentEntity == null)
                throw new InvalidOperationException("User is not linked to a government entity.");

            if (user.GovernmentEntity.Status != EntityStatus.Approved)
                throw new InvalidOperationException("Government entity is not approved.");

            var finalScore = dto.Answers.Average(a => a.Score);

            var submission = new AssessmentSubmission
            {
                GovernmentEntityId = user.GovernmentEntityId.Value,
                AssessmentType = AssessmentType.Compliance,
                FinalScore = finalScore,
                ResultLevel = GetResultLevel(finalScore),
                SubmittedByUserId = user.Id,
                SubmittedAt = DateTime.UtcNow
            };

            await _context.AssessmentSubmissions.AddAsync(submission);
            await _context.SaveChangesAsync();

            var answers = dto.Answers.Select(a => new AssessmentAnswer
            {
                AssessmentSubmissionId = submission.Id,
                QuestionKey = a.QuestionKey,
                Score = a.Score,
                Note = a.Note
            }).ToList();

            await _context.AssessmentAnswers.AddRangeAsync(answers);

            user.GovernmentEntity.ComplianceScore = finalScore;

            await _context.SaveChangesAsync();

            return "Compliance assessment submitted successfully.";
        }

        public async Task<ComplianceResultDto?> GetMyResultAsync(string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.GovernmentEntityId == null)
                throw new InvalidOperationException("User is not linked to a government entity.");

            return await GetResultByEntityIdAsync(user.GovernmentEntityId.Value);
        }

        public async Task<IEnumerable<ComplianceResultDto>> GetAllResultsAsync()
        {
            return await _context.AssessmentSubmissions
                .Where(s => s.AssessmentType == AssessmentType.Compliance)
                .Include(s => s.GovernmentEntity)
                .Include(s => s.Answers)
                .OrderByDescending(s => s.SubmittedAt)
                .Select(s => new ComplianceResultDto
                {
                    SubmissionId = s.Id,
                    GovernmentEntityId = s.GovernmentEntityId,
                    GovernmentEntityName = s.GovernmentEntity.Name,
                    FinalScore = s.FinalScore,
                    CompliancePercentage = Math.Round((s.FinalScore / 5) * 100, 2),
                    ResultLevel = s.ResultLevel,
                    SubmittedAt = s.SubmittedAt,
                    Answers = s.Answers.Select(a => new ComplianceAnswerResultDto
                    {
                        QuestionKey = a.QuestionKey,
                        Score = a.Score,
                        Note = a.Note
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ComplianceResultDto?> GetResultByEntityIdAsync(int entityId)
        {
            return await _context.AssessmentSubmissions
                .Where(s => s.AssessmentType == AssessmentType.Compliance && s.GovernmentEntityId == entityId)
                .Include(s => s.GovernmentEntity)
                .Include(s => s.Answers)
                .OrderByDescending(s => s.SubmittedAt)
                .Select(s => new ComplianceResultDto
                {
                    SubmissionId = s.Id,
                    GovernmentEntityId = s.GovernmentEntityId,
                    GovernmentEntityName = s.GovernmentEntity.Name,
                    FinalScore = s.FinalScore,
                    CompliancePercentage = Math.Round((s.FinalScore / 5) * 100, 2),
                    ResultLevel = s.ResultLevel,
                    SubmittedAt = s.SubmittedAt,
                    Answers = s.Answers.Select(a => new ComplianceAnswerResultDto
                    {
                        QuestionKey = a.QuestionKey,
                        Score = a.Score,
                        Note = a.Note
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        private static AssessmentResultLevel GetResultLevel(double score)
        {
            if (score < 2) return AssessmentResultLevel.VeryLow;
            if (score < 3) return AssessmentResultLevel.Low;
            if (score < 4) return AssessmentResultLevel.Medium;
            if (score < 4.5) return AssessmentResultLevel.Good;

            return AssessmentResultLevel.Excellent;
        }
    }
}