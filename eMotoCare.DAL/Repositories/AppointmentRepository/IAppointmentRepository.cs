﻿using eMotoCare.BO.Entities;
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
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        );

        Task<Appointment?> GetByIdAsync(Guid id);
        Task<bool> ExistsCodeAsync(string code);

        Task<IReadOnlyList<string>> GetAvailableSlotsAsync(Guid serviceCenterId, DateTime date);
        Task<bool> ExistsOverlapAsync(Guid serviceCenterId, DateTime date, string timeSlot);
    }
}
