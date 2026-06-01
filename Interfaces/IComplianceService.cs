using gov_API.Entities.Dtos.Compliance;

namespace gov_API.Interfaces
{
    public interface IComplianceService
    {
        Task<string> SubmitAsync(SubmitComplianceAssessmentDto dto, string userId);

        Task<ComplianceResultDto?> GetMyResultAsync(string userId);

        Task<IEnumerable<ComplianceResultDto>> GetAllResultsAsync();

        Task<ComplianceResultDto?> GetResultByEntityIdAsync(int entityId);
    }
}