using gov_API.DTOs.Readiness;

namespace gov_API.Interfaces
{
    public interface IReadinessService
    {
        Task<IEnumerable<ReadinessResultDto>> GetAllResultsAsync();

        Task<ReadinessResultDto?> GetResultByEntityIdAsync(int entityId);
    }
}