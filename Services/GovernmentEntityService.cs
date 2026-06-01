using gov_API.Data;
using gov_API.DTOs.GovernmentEntities;
using gov_API.Entities.Models;
using gov_API.Enums;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace gov_API.Services
{
    public class GovernmentEntityService : IGovernmentEntityService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GovernmentEntityService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> SubmitJoinRequestAsync(GovernmentEntityJoinRequestDto dto)
        {
            if (!dto.ReadinessAnswers.Any())
                throw new InvalidOperationException("Readiness answers are required.");

            foreach (var answer in dto.ReadinessAnswers)
            {
                if (string.IsNullOrWhiteSpace(answer.QuestionKey))
                    throw new InvalidOperationException("Question key is required.");

                if (answer.Score < 1 || answer.Score > 5)
                    throw new InvalidOperationException("Score must be between 1 and 5.");
            }

            var adminEmailExists = await _userManager.FindByEmailAsync(dto.AdminEmail);

            if (adminEmailExists != null)
                throw new InvalidOperationException("Admin email is already used.");

            var entityEmailExists = await _context.GovernmentEntities
                .AnyAsync(e => e.Email == dto.EntityEmail);

            if (entityEmailExists)
                throw new InvalidOperationException("Government entity email is already used.");

            var governmentEntity = new GovernmentEntity
            {
                Name = dto.EntityName,
                Email = dto.EntityEmail,
                Phone = dto.Phone,
                Address = dto.Address,
                Status = EntityStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _context.GovernmentEntities.AddAsync(governmentEntity);
            await _context.SaveChangesAsync();

            var user = new ApplicationUser
            {
                FullName = dto.AdminFullName,
                UserName = dto.AdminEmail,
                Email = dto.AdminEmail,
                EmailConfirmed = true,
                IsActive = false,
                GovernmentEntityId = governmentEntity.Id,
                CreatedAt = DateTime.UtcNow
            };

            var createUserResult = await _userManager.CreateAsync(user, dto.Password);

            if (!createUserResult.Succeeded)
            {
                _context.GovernmentEntities.Remove(governmentEntity);
                await _context.SaveChangesAsync();

                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, "EntityAdmin");

            if (!addRoleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                _context.GovernmentEntities.Remove(governmentEntity);
                await _context.SaveChangesAsync();

                var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(errors);
            }

            var finalScore = dto.ReadinessAnswers.Average(a => a.Score);

            var submission = new AssessmentSubmission
            {
                GovernmentEntityId = governmentEntity.Id,
                AssessmentType = AssessmentType.Readiness,
                FinalScore = finalScore,
                ResultLevel = GetResultLevel(finalScore),
                SubmittedByUserId = user.Id,
                SubmittedAt = DateTime.UtcNow
            };

            await _context.AssessmentSubmissions.AddAsync(submission);
            await _context.SaveChangesAsync();

            var answers = dto.ReadinessAnswers.Select(a => new AssessmentAnswer
            {
                AssessmentSubmissionId = submission.Id,
                QuestionKey = a.QuestionKey,
                Score = a.Score,
                Note = a.Note
            }).ToList();

            await _context.AssessmentAnswers.AddRangeAsync(answers);

            governmentEntity.ReadinessScore = finalScore;

            await _context.SaveChangesAsync();

            return "Join request submitted successfully. Please wait for admin approval.";
        }

        public async Task<IEnumerable<GovernmentEntityDto>> GetAllAsync()
        {
            return await _context.GovernmentEntities
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new GovernmentEntityDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    Status = e.Status,
                    ReadinessScore = e.ReadinessScore,
                    CreatedAt = e.CreatedAt,
                    ApprovedAt = e.ApprovedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<GovernmentEntityDto>> GetPendingAsync()
        {
            return await _context.GovernmentEntities
                .Where(e => e.Status == EntityStatus.Pending)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new GovernmentEntityDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    Status = e.Status,
                    ReadinessScore = e.ReadinessScore,
                    CreatedAt = e.CreatedAt,
                    ApprovedAt = e.ApprovedAt
                })
                .ToListAsync();
        }

        public async Task<GovernmentEntityDto?> GetByIdAsync(int id)
        {
            return await _context.GovernmentEntities
                .Where(e => e.Id == id)
                .Select(e => new GovernmentEntityDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Email = e.Email,
                    Phone = e.Phone,
                    Address = e.Address,
                    Status = e.Status,
                    ReadinessScore = e.ReadinessScore,
                    CreatedAt = e.CreatedAt,
                    ApprovedAt = e.ApprovedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<string> ApproveAsync(int id)
        {
            var entity = await _context.GovernmentEntities.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException("Government entity not found.");

            entity.Status = EntityStatus.Approved;
            entity.ApprovedAt = DateTime.UtcNow;

            var users = await _userManager.Users
                .Where(u => u.GovernmentEntityId == entity.Id)
                .ToListAsync();

            foreach (var user in users)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }

            await _context.SaveChangesAsync();

            return "Government entity approved successfully.";
        }

        public async Task<string> RejectAsync(int id)
        {
            var entity = await _context.GovernmentEntities.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException("Government entity not found.");

            entity.Status = EntityStatus.Rejected;

            var users = await _userManager.Users
                .Where(u => u.GovernmentEntityId == entity.Id)
                .ToListAsync();

            foreach (var user in users)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }

            await _context.SaveChangesAsync();

            return "Government entity rejected successfully.";
        }

        public async Task<string> SuspendAsync(int id)
        {
            var entity = await _context.GovernmentEntities.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException("Government entity not found.");

            entity.Status = EntityStatus.Suspended;

            var users = await _userManager.Users
                .Where(u => u.GovernmentEntityId == entity.Id)
                .ToListAsync();

            foreach (var user in users)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }

            await _context.SaveChangesAsync();

            return "Government entity suspended successfully.";
        }

        private static AssessmentResultLevel GetResultLevel(double score)
        {
            if (score < 2) return AssessmentResultLevel.VeryLow;
            if (score < 3) return AssessmentResultLevel.Low;
            if (score < 4) return AssessmentResultLevel.Medium;
            if (score < 4.5) return AssessmentResultLevel.Good;

            return AssessmentResultLevel.Excellent;
        }

        public async Task<int> GetPendingCountAsync()
        {
            return await _context.GovernmentEntities
                .CountAsync(e => e.Status == EntityStatus.Pending);
        }
    }
}