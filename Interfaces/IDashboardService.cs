using gov_API.Entities.Dtos.Dashboard;

namespace gov_API.Interfaces
{
    public interface IDashboardService
    {
        Task<AdminDashboardDto> GetAdminDashboardAsync();

        Task<EntityDashboardDto> GetEntityDashboardAsync(string userId);
    }
}
