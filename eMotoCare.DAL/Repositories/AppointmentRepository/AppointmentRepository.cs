using eMotoCare.BO.DTO.Responses;
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
                .Appointments.Include(x => x.ServiceCenter)
                .Include(x => x.Customer)
                .ThenInclude(x => x.Account)
                .Include(x => x.VehicleStage)
                .ThenInclude(x => x.Vehicle)
                .ThenInclude(x => x.Model)
                .Include(x => x.EVCheck)
                .Include(x => x.VehicleStage)
                .ThenInclude(x => x.MaintenanceStage)
                .Include(x => x.Vehicle)
                .ThenInclude(x => x.Model)
                //.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.Appointments.AnyAsync(x => x.Code == code);

        public async Task<(IReadOnlyList<Appointment> Items, long Total)> GetPagedAsync(
            string? search,
            AppointmentStatus? status,
            Guid? serviceCenterId,
            Guid? customerId,
            Guid? technicianId,
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
                .ThenInclude(x => x.Account)
                .Include(x => x.VehicleStage)
                .ThenInclude(x => x.MaintenanceStage)
                .Include(x => x.Vehicle)
                .ThenInclude(x => x.Model)
                .Include(x => x.EVCheck)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    x.Code.ToLower().Contains(s)
                    || (
                        x.Customer != null
                        && x.Customer.Account != null
                        && x.Customer.Account.Phone.ToLower().Contains(s)
                    )
                );
            }
            if (customerId.HasValue)
                q = q.Where(a => a.CustomerId == customerId.Value);
            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (serviceCenterId.HasValue)
                q = q.Where(x => x.ServiceCenterId == serviceCenterId.Value);
            if (technicianId.HasValue)
                q = q.Where(a =>
                    a.EVCheck != null && a.EVCheck.TaskExecutorId == technicianId.Value
                );
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
                .ThenInclude(x => x.EVCheckDetails)
                .FirstOrDefaultAsync(x => x.Code == code);

        public Task UpdateStatusByIdAsync(Guid id, AppointmentStatus status)
        {
            var appt = new Appointment { Id = id, Status = status };
            _context.Attach(appt);
            _context.Entry(appt).Property(a => a.Status).IsModified = true;
            return Task.CompletedTask;
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

        public async Task<(int totalAppointment, double totalRevenue)> TotalAppoinmentAndRevenue(
            Guid? serviceCenterId
        )
        {
            var query = _context.Appointments.AsQueryable();
            if (serviceCenterId.HasValue)
            {
                query = query.Where(a => a.ServiceCenterId == serviceCenterId.Value);
            }
            int totalAppointment = await query.CountAsync();
            var completedQuery = query.Where(a => a.Status == AppointmentStatus.COMPLETED);
            var totalRevenueDecimal = await completedQuery.SumAsync(a =>
                a.EVCheck != null ? a.EVCheck.TotalAmout : 0m
            );
            double totalRevenue = (double)totalRevenueDecimal;
            return (totalAppointment, totalRevenue);
        }
        public async Task<List<AppointmentDashboardMonthItem>> GetAppointmentDashboardByMonthAsync(
        Guid? serviceCenterId,
        int year
)
        {
            var q = _context.Appointments.AsNoTracking().Include(a => a.EVCheck)
                .Where(a => a.AppointmentDate.Year == year);

            if (serviceCenterId.HasValue)
                q = q.Where(a => a.ServiceCenterId == serviceCenterId.Value);

            var grouped = await q
                .GroupBy(a => a.AppointmentDate.Month)
                .Select(g => new AppointmentDashboardMonthItem
                {
                    Month = g.Key,
                    Total = g.Count(),
                    CheckedIn = g.Count(x => x.Status == AppointmentStatus.CHECKED_IN),
                    Completed = g.Count(x => x.Status == AppointmentStatus.COMPLETED),
                    WaitingForPayment = g.Count(x => x.Status == AppointmentStatus.WAITING_FOR_PAYMENT),
                    Maintenance = g.Count(x => x.Type == ServiceType.MAINTENANCE_TYPE),
                    Repair = g.Count(x => x.Type == ServiceType.REPAIR_TYPE),
                    Warranty = g.Count(x => x.Type == ServiceType.WARRANTY_TYPE),
                    Campaign = g.Count(x => x.Type == ServiceType.CAMPAIGN_TYPE),
                    Recall = g.Count(x => x.Type == ServiceType.RECALL_TYPE),
                    Revenue = g.Where(x => x.Status == AppointmentStatus.COMPLETED).Sum(x => x.EVCheck == null ? 0m : (x.EVCheck.TotalAmout ?? 0m))

                })
                .ToListAsync();
            var dict = grouped.ToDictionary(x => x.Month, x => x);
            var result = new List<AppointmentDashboardMonthItem>();

            for (int m = 1; m <= 12; m++)
            {
                if (dict.TryGetValue(m, out var item))
                    result.Add(item);
                else
                    result.Add(new AppointmentDashboardMonthItem { Month = m, Revenue = 0m });
            }

            return result;
        }
    }
}
