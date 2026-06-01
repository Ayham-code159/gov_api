using gov_API.Entities.Dtos.Reports;

namespace gov_API.Interfaces
{
    public interface IReportService
    {
        Task<PlatformSummaryReportDto> GetPlatformSummaryAsync();

        Task<GovernmentEntityReportDto?> GetEntityReportAsync(int entityId);

        Task<IEnumerable<AssessmentReportItemDto>> GetReadinessReportAsync();

        Task<IEnumerable<AssessmentReportItemDto>> GetMaturityReportAsync();

        Task<IEnumerable<AssessmentReportItemDto>> GetComplianceReportAsync();

        Task<VulnerabilityReportDto> GetVulnerabilityReportAsync();
    }
}