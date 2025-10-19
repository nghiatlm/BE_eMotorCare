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
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        );

        Task<AppointmentResponse?> GetByIdAsync(Guid id);

        Task<Guid> CreateAsync(AppointmentRequest req);
        Task UpdateAsync(Guid id, AppointmentRequest req);
        Task DeleteAsync(Guid id);

        Task<IReadOnlyList<string>> GetAvailableSlotsAsync(Guid serviceCenterId, DateTime date);
        Task<string> GetCheckinCodeAsync(Guid id);
        Task ApproveAsync(Guid id, Guid staffId);
        Task UpdateStatusAsync(Guid id, AppointmentStatus status);
        Task AssignTechnicianAsync(Guid appointmentId, Guid technicianId, Guid approveById);

        Task CheckInAsync(Guid appointmentId);

        Task<EVCheckResponse> UpsertEVCheckAsync(
            Guid appointmentId,
            EVCheckUpsertRequest req,
            Guid technicianId
        );
        Task<EVCheckResponse?> GetEVCheckAsync(Guid appointmentId);

        Task ConfirmInspectionAsync(Guid appointmentId, InspectionConfirmRequest req);

        Task StartRepairAsync(Guid appointmentId);
        Task FinishRepairAsync(Guid appointmentId);

        Task<RepairTicketResponse> GetRepairTicketAsync(Guid appointmentId);
    }
}
