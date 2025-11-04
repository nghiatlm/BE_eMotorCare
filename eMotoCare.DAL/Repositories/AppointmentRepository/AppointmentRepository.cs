using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.AppointmentRepository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<Appointment?> GetByIdAsync(Guid id) =>
            _context
                .Appointments.AsNoTracking()
                .Include(x => x.ServiceCenter)
                .Include(x => x.Customer)
                .Include(x => x.VehicleStage)
                .Include(x => x.ServiceCenterSlot)
                .Include(x => x.EVCheck)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.Appointments.AnyAsync(x => x.Code == code);

        public async Task<(IReadOnlyList<Appointment> Items, long Total)> GetPagedAsync(
            string? search,
            AppointmentStatus? status,
            Guid? serviceCenterId,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context
                .Appointments.AsNoTracking()
                .Include(x => x.ServiceCenter)
                .Include(x => x.Customer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x => x.Code.ToLower().Contains(s));
            }

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (serviceCenterId.HasValue)
                q = q.Where(x => x.ServiceCenterId == serviceCenterId.Value);
            if (fromDate.HasValue)
                q = q.Where(x => x.AppointmentDate.Date >= fromDate.Value.Date);
            if (toDate.HasValue)
                q = q.Where(x => x.AppointmentDate.Date <= toDate.Value.Date);

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.AppointmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<IReadOnlyList<string>> GetAvailableSlotsAsync(
            Guid serviceCenterId,
            DateTime date
        )
        {
            var dow = (eMotoCare.BO.Enums.DayOfWeeks)date.DayOfWeek;

            var dateSlotsQuery = _context
                .ServiceCenterSlots.AsNoTracking()
                .Where(s =>
                    s.ServiceCenterId == serviceCenterId
                    && s.IsActive
                    && s.Date == DateOnly.FromDateTime(date.Date)
                );

            var weeklySlotsQuery = _context
                .ServiceCenterSlots.AsNoTracking()
                .Where(s =>
                    s.ServiceCenterId == serviceCenterId && s.IsActive && s.DayOfWeek == dow
                );

            var dateSlots = await dateSlotsQuery.ToListAsync();
            var baseSlots = dateSlots.Count > 0 ? dateSlots : await weeklySlotsQuery.ToListAsync();

            if (baseSlots.Count == 0)
                return Array.Empty<string>();

            var slotIds = baseSlots.Select(s => s.Id).ToList();
            var bookedCounts = await _context
                .Appointments.AsNoTracking()
                .Where(a =>
                    a.ServiceCenterId == serviceCenterId
                    && a.AppointmentDate.Date == date.Date
                    && a.ServiceCenterSlotId.HasValue
                    && slotIds.Contains(a.ServiceCenterSlotId.Value)
                    && (
                        a.Status == AppointmentStatus.PENDING
                        || a.Status == AppointmentStatus.APPROVED
                        || a.Status == AppointmentStatus.CHECKED_IN
                    )
                )
                .GroupBy(a => a.ServiceCenterSlotId!.Value)
                .Select(g => new { SlotId = g.Key, Count = g.Count() })
                .ToListAsync();

            var bookedDict = bookedCounts.ToDictionary(x => x.SlotId, x => x.Count);

            var available = baseSlots
                .Where(s => s.Capacity - (bookedDict.TryGetValue(s.Id, out var c) ? c : 0) > 0)
                .OrderBy(s => s.StartTime)
                .Select(s => $"{s.StartTime:hh\\:mm}-{s.EndTime:hh\\:mm}")
                .ToList();

            return available;
        }

        public async Task<bool> ExistsOverlapAsync(
            Guid serviceCenterId,
            DateTime date,
            string timeSlot
        )
        {
            var parts = timeSlot.Split(
                '-',
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            );
            if (parts.Length != 2)
                return true;

            if (
                !TimeSpan.TryParse(parts[0], out var start)
                || !TimeSpan.TryParse(parts[1], out var end)
            )
                return true;

            var dow = (eMotoCare.BO.Enums.DayOfWeeks)date.DayOfWeek;

            var slot = await _context
                .ServiceCenterSlots.AsNoTracking()
                .Where(s =>
                    s.ServiceCenterId == serviceCenterId
                    && s.IsActive
                    && (
                        (s.Date == DateOnly.FromDateTime(date.Date))
                        || (s.Date == default && s.DayOfWeek == dow)
                    )
                    && s.StartTime == start
                    && s.EndTime == end
                )
                .OrderByDescending(s => s.Date != default)
                .FirstOrDefaultAsync();

            if (slot == null)
                return true;

            var count = await _context
                .Appointments.AsNoTracking()
                .Where(a =>
                    a.ServiceCenterId == serviceCenterId
                    && a.AppointmentDate.Date == date.Date
                    && a.ServiceCenterSlotId == slot.Id
                    && (
                        a.Status == AppointmentStatus.PENDING
                        || a.Status == AppointmentStatus.APPROVED
                        || a.Status == AppointmentStatus.CHECKED_IN
                    )
                )
                .CountAsync();

            return count >= slot.Capacity;
        }

        public Task<Appointment?> GetByCodeAsync(string code) =>
            _context
                .Appointments.Include(x => x.ServiceCenter)
                .Include(x => x.Customer)
                .Include(x => x.ServiceCenterSlot)
                .FirstOrDefaultAsync(x => x.Code == code);

        public Task UpdateStatusByIdAsync(Guid id, AppointmentStatus status)
        {
            var appt = new Appointment { Id = id, Status = status };
            _context.Attach(appt);
            _context.Entry(appt).Property(a => a.Status).IsModified = true;
            return Task.CompletedTask;
        }
    }
}
