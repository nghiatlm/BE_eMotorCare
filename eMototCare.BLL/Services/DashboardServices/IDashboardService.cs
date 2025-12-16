using eMotoCare.BO.DTO.Responses;

namespace eMototCare.BLL.Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<AppointmentDashboardResponse> GetAppointmentDashboardAsync(Guid? serviceCenterId, int? year);
    }
}