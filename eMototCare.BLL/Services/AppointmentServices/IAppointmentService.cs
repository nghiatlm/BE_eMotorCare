using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.AppointmentServices
{
    public interface IAppointmentService
    {
        Task<PageResult<AppointmentResponse>> GetPagedAsync(
            string? search,
            AppointmentStatus? status,
            Guid? serviceCenterId,
            Guid? customerId,
            Guid? technicianId,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        );

        Task<AppointmentResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(AppointmentRequest req);
        Task UpdateAsync(Guid id, AppointmentUpdateRequest req);
        Task DeleteAsync(Guid id);
        Task<IReadOnlyList<string>> GetAvailableSlotsAsync(Guid serviceCenterId, DateTime date);
        Task UpdateStatusAsync(Guid id, AppointmentStatus status);
        Task<FirstVisitVehicleInfoResponse> EnsureVehicleFromChassisAsync(string chassisNumber);
    }
}
