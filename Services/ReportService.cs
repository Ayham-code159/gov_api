using gov_API.Data;
using gov_API.Entities.Dtos.Reports;
using gov_API.Enums;
using gov_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gov_API.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PlatformSummaryReportDto> GetPlatformSummaryAsync()
        {
            var totalEntities = await _context.GovernmentEntities.CountAsync();

            var averageReadiness = totalEntities == 0
                ? 0
                : await _context.GovernmentEntities.AverageAsync(e => e.ReadinessScore);

            var averageMaturity = totalEntities == 0
                ? 0
                : await _context.GovernmentEntities.AverageAsync(e => e.MaturityScore);

            var averageCompliance = totalEntities == 0
                ? 0
                : await _context.GovernmentEntities.AverageAsync(e => e.ComplianceScore);

            return new PlatformSummaryReportDto
            {
                TotalEntities = totalEntities,

                PendingEntities = await _context.GovernmentEntities
                    .CountAsync(e => e.Status == EntityStatus.Pending),

                ApprovedEntities = await _context.GovernmentEntities
                    .CountAsync(e => e.Status == EntityStatus.Approved),

                RejectedEntities = await _context.GovernmentEntities
                    .CountAsync(e => e.Status == EntityStatus.Rejected),

                SuspendedEntities = await _context.GovernmentEntities
                    .CountAsync(e => e.Status == EntityStatus.Suspended),

                AverageReadinessScore = Math.Round(averageReadiness, 2),
                AverageMaturityScore = Math.Round(averageMaturity, 2),
                AverageComplianceScore = Math.Round(averageCompliance, 2),

                TotalVulnerabilities = await _context.Vulnerabilities.CountAsync(),

                OpenVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.Status == VulnerabilityStatus.Open),

                InProgressVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.Status == VulnerabilityStatus.InProgress),

                ResolvedVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.Status == VulnerabilityStatus.Resolved),

                ClosedVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.Status == VulnerabilityStatus.Closed),

                CriticalVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.Severity == VulnerabilitySeverity.Critical),

                HighVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.Severity == VulnerabilitySeverity.High),

                TotalSupportRequests = await _context.SupportRequests.CountAsync(),

                OpenSupportRequests = await _context.SupportRequests
                    .CountAsync(r => r.Status == SupportRequestStatus.Open),

                InProgressSupportRequests = await _context.SupportRequests
                    .CountAsync(r => r.Status == SupportRequestStatus.InProgress),

                ClosedSupportRequests = await _context.SupportRequests
                    .CountAsync(r => r.Status == SupportRequestStatus.Closed)
            };
        }

        public async Task<GovernmentEntityReportDto?> GetEntityReportAsync(int entityId)
        {
            var entity = await _context.GovernmentEntities
                .FirstOrDefaultAsync(e => e.Id == entityId);

            if (entity == null)
                return null;

            return new GovernmentEntityReportDto
            {
                GovernmentEntityId = entity.Id,
                GovernmentEntityName = entity.Name,
                Email = entity.Email,
                Phone = entity.Phone,
                Address = entity.Address,
                Status = entity.Status,
                ReadinessScore = entity.ReadinessScore,
                MaturityScore = entity.MaturityScore,
                ComplianceScore = entity.ComplianceScore,
                CreatedAt = entity.CreatedAt,
                ApprovedAt = entity.ApprovedAt,

                TotalVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.GovernmentEntityId == entityId),

                OpenVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.GovernmentEntityId == entityId && v.Status == VulnerabilityStatus.Open),

                InProgressVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.GovernmentEntityId == entityId && v.Status == VulnerabilityStatus.InProgress),

                ResolvedVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.GovernmentEntityId == entityId && v.Status == VulnerabilityStatus.Resolved),

                ClosedVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.GovernmentEntityId == entityId && v.Status == VulnerabilityStatus.Closed),

                CriticalVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.GovernmentEntityId == entityId && v.Severity == VulnerabilitySeverity.Critical),

                HighVulnerabilities = await _context.Vulnerabilities
                    .CountAsync(v => v.GovernmentEntityId == entityId && v.Severity == VulnerabilitySeverity.High),

                TotalSupportRequests = await _context.SupportRequests
                    .CountAsync(r => r.GovernmentEntityId == entityId),

                OpenSupportRequests = await _context.SupportRequests
                    .CountAsync(r => r.GovernmentEntityId == entityId && r.Status == SupportRequestStatus.Open),

                InProgressSupportRequests = await _context.SupportRequests
                    .CountAsync(r => r.GovernmentEntityId == entityId && r.Status == SupportRequestStatus.InProgress),

                ClosedSupportRequests = await _context.SupportRequests
                    .CountAsync(r => r.GovernmentEntityId == entityId && r.Status == SupportRequestStatus.Closed)
            };
        }

        public async Task<IEnumerable<AssessmentReportItemDto>> GetReadinessReportAsync()
        {
            return await GetAssessmentReportAsync(AssessmentType.Readiness);
        }

        public async Task<IEnumerable<AssessmentReportItemDto>> GetMaturityReportAsync()
        {
            return await GetAssessmentReportAsync(AssessmentType.Maturity);
        }

        public async Task<IEnumerable<AssessmentReportItemDto>> GetComplianceReportAsync()
        {
            return await GetAssessmentReportAsync(AssessmentType.Compliance);
        }

        public async Task<VulnerabilityReportDto> GetVulnerabilityReportAsync()
        {
            var vulnerabilities = _context.Vulnerabilities;

            return new VulnerabilityReportDto
            {
                TotalVulnerabilities = await vulnerabilities.CountAsync(),

                OpenVulnerabilities = await vulnerabilities
                    .CountAsync(v => v.Status == VulnerabilityStatus.Open),

                InProgressVulnerabilities = await vulnerabilities
                    .CountAsync(v => v.Status == VulnerabilityStatus.InProgress),

                ResolvedVulnerabilities = await vulnerabilities
                    .CountAsync(v => v.Status == VulnerabilityStatus.Resolved),

                ClosedVulnerabilities = await vulnerabilities
                    .CountAsync(v => v.Status == VulnerabilityStatus.Closed),

                LowVulnerabilities = await vulnerabilities
                    .CountAsync(v => v.Severity == VulnerabilitySeverity.Low),

                MediumVulnerabilities = await vulnerabilities
                    .CountAsync(v => v.Severity == VulnerabilitySeverity.Medium),

                HighVulnerabilities = await vulnerabilities
                    .CountAsync(v => v.Severity == VulnerabilitySeverity.High),

                CriticalVulnerabilities = await vulnerabilities
                    .CountAsync(v => v.Severity == VulnerabilitySeverity.Critical),

                ByEntity = await _context.GovernmentEntities
                    .Select(e => new VulnerabilityByEntityDto
                    {
                        GovernmentEntityId = e.Id,
                        GovernmentEntityName = e.Name,

                        Total = _context.Vulnerabilities
                            .Count(v => v.GovernmentEntityId == e.Id),

                        Open = _context.Vulnerabilities
                            .Count(v => v.GovernmentEntityId == e.Id && v.Status == VulnerabilityStatus.Open),

                        InProgress = _context.Vulnerabilities
                            .Count(v => v.GovernmentEntityId == e.Id && v.Status == VulnerabilityStatus.InProgress),

                        Resolved = _context.Vulnerabilities
                            .Count(v => v.GovernmentEntityId == e.Id && v.Status == VulnerabilityStatus.Resolved),

                        Closed = _context.Vulnerabilities
                            .Count(v => v.GovernmentEntityId == e.Id && v.Status == VulnerabilityStatus.Closed),

                        Critical = _context.Vulnerabilities
                            .Count(v => v.GovernmentEntityId == e.Id && v.Severity == VulnerabilitySeverity.Critical),

                        High = _context.Vulnerabilities
                            .Count(v => v.GovernmentEntityId == e.Id && v.Severity == VulnerabilitySeverity.High)
                    })
                    .ToListAsync()
            };
        }

        private async Task<IEnumerable<AssessmentReportItemDto>> GetAssessmentReportAsync(AssessmentType type)
        {
            return await _context.AssessmentSubmissions
                .Where(s => s.AssessmentType == type)
                .Include(s => s.GovernmentEntity)
                .OrderByDescending(s => s.SubmittedAt)
                .Select(s => new AssessmentReportItemDto
                {
                    SubmissionId = s.Id,
                    GovernmentEntityId = s.GovernmentEntityId,
                    GovernmentEntityName = s.GovernmentEntity.Name,
                    AssessmentType = s.AssessmentType,
                    FinalScore = s.FinalScore,
                    Percentage = Math.Round((s.FinalScore / 5) * 100, 2),
                    ResultLevel = s.ResultLevel,
                    SubmittedAt = s.SubmittedAt
                })
                .ToListAsync();
        }
    }
}