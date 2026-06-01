using gov_API.Entities.Dtos.Policies;

namespace gov_API.Interfaces
{
    public interface IPolicyService
    {
        Task<IEnumerable<PolicyDto>> GetAllAsync();

        Task<PolicyDto?> GetByIdAsync(int id);

        Task<string> CreateAsync(CreatePolicyDto dto, string userId);

        Task<string> UpdateAsync(int id, UpdatePolicyDto dto);

        Task<string> DeleteAsync(int id);
    }

}
