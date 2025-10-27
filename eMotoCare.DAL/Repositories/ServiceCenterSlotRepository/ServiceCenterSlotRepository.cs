using eMotoCare.BO.Entities;
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

        public Task<List<ServiceCenterSlot>> GetByServiceCenterAsync(Guid serviceCenterId) =>
            _context
                .ServiceCenterSlots.AsNoTracking()
                .Where(x => x.ServiceCenterId == serviceCenterId)
                .OrderBy(x => x.DayOfWeek)
                .ThenBy(x => x.StartTime)
                .ToListAsync();

        public async Task<bool> HasOverlapAsync(
            Guid serviceCenterId,
            DayOfWeek dayOfWeek,
            TimeSpan start,
            TimeSpan end,
            Guid? excludeId = null
        )
        {
            var q = _context
                .ServiceCenterSlots.AsNoTracking()
                .Where(x => x.ServiceCenterId == serviceCenterId && x.DayOfWeek == dayOfWeek);

            if (excludeId.HasValue)
                q = q.Where(x => x.Id != excludeId.Value);

            return await q.AnyAsync(x => start < x.EndTime && end > x.StartTime);
        }

        public async Task<int> CountBookingsAsync(Guid serviceCenterId, Guid slotId, DateOnly date)
        {
            return await _context
                .Appointments.AsNoTracking()
                .Where(a =>
                    a.ServiceCenterId == serviceCenterId
                    && a.ServiceCenterSlotId == slotId
                    && DateOnly.FromDateTime(a.AppointmentDate.Date) == date
                )
                .CountAsync();
        }
    }
}
