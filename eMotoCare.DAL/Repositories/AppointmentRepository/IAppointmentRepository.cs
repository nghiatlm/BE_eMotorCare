using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.AppointmentRepository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<(IReadOnlyList<Appointment> Items, long Total)> GetPagedAsync(
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

        Task<List<AppointmentDashboardMonthItem>> GetAppointmentDashboardByMonthAsync(
            Guid? serviceCenterId,
            int year
        );
        Task<Appointment?> GetByCodeAsync(string code);
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<bool> ExistsCodeAsync(string code);
        Task<IReadOnlyList<string>> GetAvailableSlotsAsync(Guid serviceCenterId, DateTime date);
        Task UpdateStatusByIdAsync(Guid appointmentId, AppointmentStatus appointmentStatus);
        Task<List<Appointment>> GetByVehicleIdAsync(Guid vehicleId);
        Task<(int totalAppointment, double totalRevenue)> TotalAppoinmentAndRevenue(
            Guid? serviceCenterId
        );
    }
}
