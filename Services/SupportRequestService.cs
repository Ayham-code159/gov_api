using gov_API.Data;
using gov_API.Entities.Dtos.SupportRequests;
using gov_API.Entities.Models;
using gov_API.Enums;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace gov_API.Services
{
    public class SupportRequestService : ISupportRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupportRequestService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> CreateAsync(CreateSupportRequestDto dto, string userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Subject))
                throw new InvalidOperationException("Subject is required.");

            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new InvalidOperationException("Message is required.");

            var user = await _userManager.Users
                .Include(u => u.GovernmentEntity)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.GovernmentEntityId == null || user.GovernmentEntity == null)
                throw new InvalidOperationException("User is not linked to a government entity.");

            if (user.GovernmentEntity.Status != EntityStatus.Approved)
                throw new InvalidOperationException("Government entity is not approved.");

            var request = new SupportRequest
            {
                GovernmentEntityId = user.GovernmentEntityId.Value,
                CreatedByUserId = user.Id,
                Subject = dto.Subject,
                Message = dto.Message,
                Status = SupportRequestStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            await _context.SupportRequests.AddAsync(request);
            await _context.SaveChangesAsync();

            return "Support request created successfully.";
        }

        public async Task<IEnumerable<SupportRequestDto>> GetAllAsync()
        {
            return await _context.SupportRequests
                .Include(r => r.GovernmentEntity)
                .Include(r => r.CreatedByUser)
                .Include(r => r.Replies)
                    .ThenInclude(reply => reply.SenderUser)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => ToDto(r))
                .ToListAsync();
        }

        public async Task<IEnumerable<SupportRequestDto>> GetMyAsync(string userId)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.GovernmentEntityId == null)
                throw new InvalidOperationException("User is not linked to a government entity.");

            return await _context.SupportRequests
                .Where(r => r.GovernmentEntityId == user.GovernmentEntityId.Value)
                .Include(r => r.GovernmentEntity)
                .Include(r => r.CreatedByUser)
                .Include(r => r.Replies)
                    .ThenInclude(reply => reply.SenderUser)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => ToDto(r))
                .ToListAsync();
        }

        public async Task<SupportRequestDto?> GetByIdAsync(int id, string userId, bool isPlatformAdmin)
        {
            var request = await _context.SupportRequests
                .Include(r => r.GovernmentEntity)
                .Include(r => r.CreatedByUser)
                .Include(r => r.Replies)
                    .ThenInclude(reply => reply.SenderUser)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return null;

            if (!isPlatformAdmin)
            {
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    throw new UnauthorizedAccessException("User not found.");

                if (user.GovernmentEntityId != request.GovernmentEntityId)
                    throw new UnauthorizedAccessException("You cannot view this support request.");
            }

            return ToDto(request);
        }

        public async Task<string> ReplyAsync(int requestId, CreateSupportReplyDto dto, string userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new InvalidOperationException("Message is required.");

            var request = await _context.SupportRequests
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new KeyNotFoundException("Support request not found.");

            if (request.Status == SupportRequestStatus.Closed)
                throw new InvalidOperationException("Cannot reply to a closed support request.");

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            if (!roles.Contains("PlatformAdmin"))
            {
                if (user.GovernmentEntityId != request.GovernmentEntityId)
                    throw new UnauthorizedAccessException("You cannot reply to this support request.");
            }

            var reply = new SupportReply
            {
                SupportRequestId = requestId,
                SenderUserId = user.Id,
                Message = dto.Message,
                CreatedAt = DateTime.UtcNow
            };

            await _context.SupportReplies.AddAsync(reply);

            if (request.Status == SupportRequestStatus.Open)
                request.Status = SupportRequestStatus.InProgress;

            await _context.SaveChangesAsync();

            return "Reply added successfully.";
        }

        public async Task<string> CloseAsync(int requestId)
        {
            var request = await _context.SupportRequests
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new KeyNotFoundException("Support request not found.");

            request.Status = SupportRequestStatus.Closed;
            request.ClosedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return "Support request closed successfully.";
        }

        public async Task<string> DeleteAsync(int requestId)
        {
            var request = await _context.SupportRequests
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                throw new KeyNotFoundException("Support request not found.");

            _context.SupportRequests.Remove(request);
            await _context.SaveChangesAsync();

            return "Support request deleted successfully.";
        }

        private static SupportRequestDto ToDto(SupportRequest request)
        {
            return new SupportRequestDto
            {
                Id = request.Id,
                GovernmentEntityId = request.GovernmentEntityId,
                GovernmentEntityName = request.GovernmentEntity.Name,
                CreatedByUserId = request.CreatedByUserId,
                CreatedByFullName = request.CreatedByUser.FullName,
                Subject = request.Subject,
                Message = request.Message,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                ClosedAt = request.ClosedAt,
                Replies = request.Replies
                    .OrderBy(r => r.CreatedAt)
                    .Select(r => new SupportReplyDto
                    {
                        Id = r.Id,
                        SupportRequestId = r.SupportRequestId,
                        SenderUserId = r.SenderUserId,
                        SenderFullName = r.SenderUser.FullName,
                        SenderEmail = r.SenderUser.Email ?? string.Empty,
                        Message = r.Message,
                        CreatedAt = r.CreatedAt
                    })
                    .ToList()
            };
        }
    }

}
