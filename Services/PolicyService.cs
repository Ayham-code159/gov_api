using gov_API.Data;
using gov_API.Entities.Dtos.Policies;
using gov_API.Entities.Models;
using gov_API.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace gov_API.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly ApplicationDbContext _context;

        public PolicyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PolicyDto>> GetAllAsync()
        {
            return await _context.Policies
                .Include(p => p.CreatedByUser)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PolicyDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Category = p.Category,
                    CreatedByUserId = p.CreatedByUserId,
                    CreatedByFullName = p.CreatedByUser.FullName,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<PolicyDto?> GetByIdAsync(int id)
        {
            return await _context.Policies
                .Include(p => p.CreatedByUser)
                .Where(p => p.Id == id)
                .Select(p => new PolicyDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Category = p.Category,
                    CreatedByUserId = p.CreatedByUserId,
                    CreatedByFullName = p.CreatedByUser.FullName,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<string> CreateAsync(CreatePolicyDto dto, string userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new InvalidOperationException("Title is required.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new InvalidOperationException("Description is required.");

            if (string.IsNullOrWhiteSpace(dto.Category))
                throw new InvalidOperationException("Category is required.");

            var policy = new Policy
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                CreatedByUserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Policies.AddAsync(policy);
            await _context.SaveChangesAsync();

            return "Policy created successfully.";
        }

        public async Task<string> UpdateAsync(int id, UpdatePolicyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new InvalidOperationException("Title is required.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new InvalidOperationException("Description is required.");

            if (string.IsNullOrWhiteSpace(dto.Category))
                throw new InvalidOperationException("Category is required.");

            var policy = await _context.Policies.FindAsync(id);

            if (policy == null)
                throw new KeyNotFoundException("Policy not found.");

            policy.Title = dto.Title;
            policy.Description = dto.Description;
            policy.Category = dto.Category;
            policy.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return "Policy updated successfully.";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var policy = await _context.Policies.FindAsync(id);

            if (policy == null)
                throw new KeyNotFoundException("Policy not found.");

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();

            return "Policy deleted successfully.";
        }
    }

}
