
using eMotoCare.BO.DTO.Dashboard;

namespace eMototCare.BLL.Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<Overview?> GetOverviewAsync(Guid? serviceCenterId);
    }
}