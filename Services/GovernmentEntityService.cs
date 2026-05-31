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
            var emailExists = await _userManager.FindByEmailAsync(dto.AdminEmail);

            if (emailExists != null)
                throw new InvalidOperationException("Admin email is already used.");

            var entityEmailExists = await _context.GovernmentEntities
                .AnyAsync(e => e.Email == dto.EntityEmail);

            if (entityEmailExists)
                throw new InvalidOperationException("Government entity email is already used.");

            foreach (var answer in dto.ReadinessAnswers)
            {
                if (answer.Score < 1 || answer.Score > 5)
                    throw new InvalidOperationException("Answer score must be between 1 and 5.");
            }

            var governmentEntity = new GovernmentEntity
            {
                Name = dto.EntityName,
                Email = dto.EntityEmail,
                Phone = dto.Phone,
                Address = dto.Address,
                Status = EntityStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                SecurityStatus = "Pending"
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

            await _userManager.AddToRoleAsync(user, "EntityAdmin");

            if (dto.ReadinessAnswers.Any())
            {
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
                    AssessmentQuestionId = a.QuestionId,
                    Score = a.Score,
                    Note = a.Note
                }).ToList();

                await _context.AssessmentAnswers.AddRangeAsync(answers);

                governmentEntity.ReadinessScore = finalScore;

                await _context.SaveChangesAsync();
            }

            return "Join request submitted successfully. Please wait for admin approval.";
        }

        public async Task<IEnumerable<GovernmentEntityDto>> GetAllAsync()
        {
            return await _context.GovernmentEntities
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => ToDto(e))
                .ToListAsync();
        }

        public async Task<IEnumerable<GovernmentEntityDto>> GetPendingAsync()
        {
            return await _context.GovernmentEntities
                .Where(e => e.Status == EntityStatus.Pending)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => ToDto(e))
                .ToListAsync();
        }

        public async Task<GovernmentEntityDto?> GetByIdAsync(int id)
        {
            var entity = await _context.GovernmentEntities.FindAsync(id);

            if (entity == null)
                return null;

            return ToDto(entity);
        }

        public async Task<string> ApproveAsync(int id)
        {
            var entity = await _context.GovernmentEntities.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException("Government entity not found.");

            entity.Status = EntityStatus.Approved;
            entity.ApprovedAt = DateTime.UtcNow;
            entity.SecurityStatus = "Approved";

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
            entity.SecurityStatus = "Rejected";

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
            entity.SecurityStatus = "Suspended";

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

        private static GovernmentEntityDto ToDto(GovernmentEntity entity)
        {
            return new GovernmentEntityDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                Address = entity.Address,
                Status = entity.Status,
                ReadinessScore = entity.ReadinessScore,
                CompliancePercentage = entity.CompliancePercentage,
                MaturityScore = entity.MaturityScore,
                CreatedAt = entity.CreatedAt,
                ApprovedAt = entity.ApprovedAt
            };
        }
    }
}