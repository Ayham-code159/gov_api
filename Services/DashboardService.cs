using gov_API.Data;
using gov_API.Entities.Dtos.Dashboard;
using gov_API.Entities.Models;
using gov_API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using gov_API.Enums;

namespace gov_API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var totalEntities = await _context.GovernmentEntities.CountAsync();

            var pendingEntities = await _context.GovernmentEntities
                .CountAsync(e => e.Status == EntityStatus.Pending);

            var approvedEntities = await _context.GovernmentEntities
                .CountAsync(e => e.Status == EntityStatus.Approved);

            var rejectedEntities = await _context.GovernmentEntities
                .CountAsync(e => e.Status == EntityStatus.Rejected);

            var suspendedEntities = await _context.GovernmentEntities
                .CountAsync(e => e.Status == EntityStatus.Suspended);

            var averageReadiness = totalEntities == 0
                ? 0
                : await _context.GovernmentEntities
                    .AverageAsync(e => e.ReadinessScore);

            return new AdminDashboardDto
            {
                TotalGovernmentEntities = totalEntities,
                PendingGovernmentEntities = pendingEntities,
                ApprovedGovernmentEntities = approvedEntities,
                RejectedGovernmentEntities = rejectedEntities,
                SuspendedGovernmentEntities = suspendedEntities,
                AverageReadinessScore = Math.Round(averageReadiness, 2),
                TotalJoinRequests = totalEntities
            };
        }

        public async Task<EntityDashboardDto> GetEntityDashboardAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.GovernmentEntity)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");

            if (user.GovernmentEntity == null)
                throw new InvalidOperationException("User is not linked to a government entity.");

            var entity = user.GovernmentEntity;

            return new EntityDashboardDto
            {
                GovernmentEntityId = entity.Id,
                GovernmentEntityName = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                Address = entity.Address,
                Status = entity.Status,
                ReadinessScore = entity.ReadinessScore,
                CreatedAt = entity.CreatedAt,
                ApprovedAt = entity.ApprovedAt
            };
        }
    }

}
