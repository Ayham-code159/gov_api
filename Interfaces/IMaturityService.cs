using gov_API.Entities.Dtos.Maturity;

namespace gov_API.Interfaces
{
    public interface IMaturityService
    {
        Task<string> SubmitAsync(SubmitMaturityAssessmentDto dto, string userId);

        Task<MaturityResultDto?> GetMyResultAsync(string userId);

        Task<IEnumerable<MaturityResultDto>> GetAllResultsAsync();

        Task<MaturityResultDto?> GetResultByEntityIdAsync(int entityId);
    }

}
