using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ServiceCenterSlotRepository
{
    public class ServiceCenterSlotRepository
        : GenericRepository<ServiceCenterSlot>,
            IServiceCenterSlotRepository
    {
        public ServiceCenterSlotRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<List<ServiceCenterSlot>> GetByServiceCenterAsync(Guid serviceCenterId) =>
            await _context
                .ServiceCenterSlots.AsNoTracking()
                .Where(x => x.ServiceCenterId == serviceCenterId)
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.SlotTime)
                .ToListAsync();

        public async Task<bool> ExistsSlotAsync(
            Guid scId,
            DateOnly date,
            DayOfWeeks dow,
            SlotTime slot
        )
        {
            return await _context
                .ServiceCenterSlots.AsNoTracking()
                .AnyAsync(s =>
                    s.ServiceCenterId == scId
                    && s.IsActive
                    && (s.Date == date || (s.Date == default && s.DayOfWeek == dow))
                    && s.SlotTime == slot
                );
        }

        public async Task<int> CountBookingsAsync(
            Guid serviceCenterId,
            DateOnly date,
            SlotTime slot
        )
        {
            return await _context
                .Appointments.AsNoTracking()
                .Where(a =>
                    a.ServiceCenterId == serviceCenterId
                    && DateOnly.FromDateTime(a.AppointmentDate.Date) == date
                    && a.SlotTime == slot
                    && (
                        a.Status == AppointmentStatus.PENDING
                        || a.Status == AppointmentStatus.APPROVED
                        || a.Status == AppointmentStatus.CHECKED_IN
                    )
                )
                .CountAsync();
        }

        public Task<List<ServiceCenterSlot>> GetByServiceCenterOnDateAsync(
            Guid scId,
            DateOnly date
        ) =>
            _context
                .ServiceCenterSlots.AsNoTracking()
                .Where(x => x.ServiceCenterId == scId && x.Date == date && x.IsActive)
                .OrderBy(x => x.SlotTime)
                .ToListAsync();
    }
}
