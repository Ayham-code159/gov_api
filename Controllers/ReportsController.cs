using gov_API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gov_API.Controllers
{
    [Route("api/reports")]
    [ApiController]
    [Authorize(Roles = "PlatformAdmin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("platform-summary")]
        public async Task<IActionResult> GetPlatformSummary()
        {
            var result = await _reportService.GetPlatformSummaryAsync();
            return Ok(result);
        }

        [HttpGet("entity/{entityId:int}")]
        public async Task<IActionResult> GetEntityReport(int entityId)
        {
            var result = await _reportService.GetEntityReportAsync(entityId);

            if (result == null)
                return NotFound(new { message = "Government entity not found." });

            return Ok(result);
        }

        [HttpGet("readiness")]
        public async Task<IActionResult> GetReadinessReport()
        {
            var result = await _reportService.GetReadinessReportAsync();
            return Ok(result);
        }

        [HttpGet("maturity")]
        public async Task<IActionResult> GetMaturityReport()
        {
            var result = await _reportService.GetMaturityReportAsync();
            return Ok(result);
        }

        [HttpGet("compliance")]
        public async Task<IActionResult> GetComplianceReport()
        {
            var result = await _reportService.GetComplianceReportAsync();
            return Ok(result);
        }

        [HttpGet("vulnerabilities")]
        public async Task<IActionResult> GetVulnerabilityReport()
        {
            var result = await _reportService.GetVulnerabilityReportAsync();
            return Ok(result);
        }
    }
}