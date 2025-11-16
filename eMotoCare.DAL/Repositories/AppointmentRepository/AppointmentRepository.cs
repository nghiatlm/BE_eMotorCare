using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
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
                .Include(x => x.EVCheck)
                .Include(x => x.VehicleStage)
                .ThenInclude(vs => vs.MaintenanceStage)
                .Include(x => x.Vehicle)
                .ThenInclude(v => v.Model)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.Appointments.AnyAsync(x => x.Code == code);

        public async Task<(IReadOnlyList<Appointment> Items, long Total)> GetPagedAsync(
            string? search,
            AppointmentStatus? status,
            Guid? serviceCenterId,
            Guid? customerId,
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
                .Include(x => x.VehicleStage)
                .ThenInclude(vs => vs.MaintenanceStage)
                .Include(x => x.Vehicle)
                .ThenInclude(v => v.Model)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x => x.Code.ToLower().Contains(s));
            }
            if (customerId.HasValue)
                q = q.Where(a => a.CustomerId == customerId.Value);
            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (serviceCenterId.HasValue)
                q = q.Where(x => x.ServiceCenterId == serviceCenterId.Value);
            if (fromDate.HasValue)
                q = q.Where(x => x.AppointmentDate.Date >= fromDate.Value.Date);
            if (toDate.HasValue)
                q = q.Where(x => x.AppointmentDate.Date <= toDate.Value.Date);

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.CreatedAt)
                .ThenByDescending(x => x.AppointmentDate)
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
            var d = DateOnly.FromDateTime(date.Date);
            var dow = (DayOfWeeks)date.DayOfWeek;

            var baseSlots = await _context
                .ServiceCenterSlots.AsNoTracking()
                .Where(s =>
                    s.ServiceCenterId == serviceCenterId
                    && s.IsActive
                    && (s.Date == d || (s.Date == default && s.DayOfWeek == dow))
                )
                .ToListAsync();

            if (baseSlots.Count == 0)
                return Array.Empty<string>();

            var results = new List<string>();
            foreach (var s in baseSlots.OrderBy(x => x.SlotTime))
            {
                var booked = await _context
                    .Appointments.AsNoTracking()
                    .CountAsync(a =>
                        a.ServiceCenterId == serviceCenterId
                        && DateOnly.FromDateTime(a.AppointmentDate.Date) == d
                        && a.SlotTime == s.SlotTime
                        && (
                            a.Status == AppointmentStatus.PENDING
                            || a.Status == AppointmentStatus.APPROVED
                            || a.Status == AppointmentStatus.CHECKED_IN
                        )
                    );

                if (booked < s.Capacity)
                {
                    // Ánh xạ SlotTime -> string hiển thị (không dùng extension)
                    var text = s.SlotTime switch
                    {
                        SlotTime.H07_08 => "07:00-08:00",
                        SlotTime.H08_09 => "08:00-09:00",
                        SlotTime.H09_10 => "09:00-10:00",
                        SlotTime.H10_11 => "10:00-11:00",
                        SlotTime.H11_12 => "11:00-12:00",
                        SlotTime.H13_14 => "13:00-14:00",
                        SlotTime.H14_15 => "14:00-15:00",
                        SlotTime.H15_16 => "15:00-16:00",
                        SlotTime.H16_17 => "16:00-17:00",
                        SlotTime.H17_18 => "17:00-18:00",
                        _ => "UNKNOWN",
                    };
                    results.Add(text);
                }
            }
            return results;
        }

        public Task<Appointment?> GetByCodeAsync(string code) =>
            _context
                .Appointments.Include(x => x.ServiceCenter)
                .Include(x => x.Customer)
                .Include(x => x.EVCheck)
                .FirstOrDefaultAsync(x => x.Code == code);

        public Task UpdateStatusByIdAsync(Guid id, AppointmentStatus status)
        {
            var appt = new Appointment { Id = id, Status = status };
            _context.Attach(appt);
            _context.Entry(appt).Property(a => a.Status).IsModified = true;
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Appointment>> GetByTechnicianIdAsync(Guid technicianId)
        {
            return await _context
                .Appointments.Include(a => a.Customer)
                .Include(a => a.ServiceCenter)
                .Include(a => a.VehicleStage)
                .Include(a => a.EVCheck)
                .Include(a => a.Vehicle)
                .Where(a => a.EVCheck != null && a.EVCheck.TaskExecutorId == technicianId)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<Appointment>> GetByVehicleIdAsync(Guid vehicleId)
        {
            return _context
                .Appointments.AsNoTracking()
                .Include(a => a.ServiceCenter)
                .Include(a => a.Campaign)
                .Where(a => a.VehicleId == vehicleId)
                .OrderByDescending(a => a.CreatedAt)
                .ThenByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}
