using gov_API.Entities.Dtos.SupportRequests;

namespace gov_API.Interfaces
{
    public interface ISupportRequestService
    {
        Task<string> CreateAsync(CreateSupportRequestDto dto, string userId);

        Task<IEnumerable<SupportRequestDto>> GetAllAsync();

        Task<IEnumerable<SupportRequestDto>> GetMyAsync(string userId);

        Task<SupportRequestDto?> GetByIdAsync(int id, string userId, bool isPlatformAdmin);

        Task<string> ReplyAsync(int requestId, CreateSupportReplyDto dto, string userId);

        Task<string> CloseAsync(int requestId);

        Task<string> DeleteAsync(int requestId);
    }

}
