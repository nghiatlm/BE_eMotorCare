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
            var slots = new[] { "08:00", "09:00", "10:00", "14:00", "15:00", "16:00" };

            var booked = await _context
                .Appointments.AsNoTracking()
                .Where(x =>
                    x.ServiceCenterId == serviceCenterId
                    && x.AppointmentDate.Date == date.Date
                    && (
                        x.Status == AppointmentStatus.PENDING
                        || x.Status == AppointmentStatus.APPROVED
                        || x.Status == AppointmentStatus.IN_SERVICE
                    )
                )
                .Select(x => x.TimeSlot)
                .ToListAsync();

            return slots.Where(s => !booked.Contains(s)).ToList();
        }

        public Task<bool> ExistsOverlapAsync(
            Guid serviceCenterId,
            DateTime date,
            string timeSlot
        ) =>
            _context.Appointments.AnyAsync(x =>
                x.ServiceCenterId == serviceCenterId
                && x.AppointmentDate.Date == date.Date
                && x.TimeSlot == timeSlot
                && (
                    x.Status == AppointmentStatus.PENDING
                    || x.Status == AppointmentStatus.APPROVED
                    || x.Status == AppointmentStatus.IN_SERVICE
                )
            );

        public Task<Appointment?> GetByCodeAsync(string code) =>
            _context
                .Appointments.Include(x => x.ServiceCenter)
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Code == code);
    }
}
