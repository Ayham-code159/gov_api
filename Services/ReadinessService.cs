using gov_API.Data;
using gov_API.DTOs.Readiness;
using gov_API.Enums;
using gov_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gov_API.Services
{
    public class ReadinessService : IReadinessService
    {
        private readonly ApplicationDbContext _context;

        public ReadinessService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReadinessResultDto>> GetAllResultsAsync()
        {
            return await _context.AssessmentSubmissions
                .Where(s => s.AssessmentType == AssessmentType.Readiness)
                .Include(s => s.GovernmentEntity)
                .Include(s => s.Answers)
                .OrderByDescending(s => s.SubmittedAt)
                .Select(s => new ReadinessResultDto
                {
                    SubmissionId = s.Id,
                    GovernmentEntityId = s.GovernmentEntityId,
                    GovernmentEntityName = s.GovernmentEntity.Name,
                    FinalScore = s.FinalScore,
                    ResultLevel = s.ResultLevel,
                    SubmittedAt = s.SubmittedAt,
                    Answers = s.Answers.Select(a => new ReadinessAnswerResultDto
                    {
                        QuestionKey = a.QuestionKey,
                        Score = a.Score,
                        Note = a.Note
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<ReadinessResultDto?> GetResultByEntityIdAsync(int entityId)
        {
            return await _context.AssessmentSubmissions
                .Where(s => s.AssessmentType == AssessmentType.Readiness && s.GovernmentEntityId == entityId)
                .Include(s => s.GovernmentEntity)
                .Include(s => s.Answers)
                .OrderByDescending(s => s.SubmittedAt)
                .Select(s => new ReadinessResultDto
                {
                    SubmissionId = s.Id,
                    GovernmentEntityId = s.GovernmentEntityId,
                    GovernmentEntityName = s.GovernmentEntity.Name,
                    FinalScore = s.FinalScore,
                    ResultLevel = s.ResultLevel,
                    SubmittedAt = s.SubmittedAt,
                    Answers = s.Answers.Select(a => new ReadinessAnswerResultDto
                    {
                        QuestionKey = a.QuestionKey,
                        Score = a.Score,
                        Note = a.Note
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}