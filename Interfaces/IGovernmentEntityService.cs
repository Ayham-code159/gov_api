using gov_API.DTOs.GovernmentEntities;

namespace gov_API.Interfaces
{
    public interface IGovernmentEntityService
    {
        Task<string> SubmitJoinRequestAsync(GovernmentEntityJoinRequestDto dto);

        Task<IEnumerable<GovernmentEntityDto>> GetAllAsync();

        Task<IEnumerable<GovernmentEntityDto>> GetPendingAsync();

        Task<GovernmentEntityDto?> GetByIdAsync(int id);

        Task<string> ApproveAsync(int id);

        Task<string> RejectAsync(int id);

        Task<string> SuspendAsync(int id);

        Task<int> GetPendingCountAsync();
    }
}