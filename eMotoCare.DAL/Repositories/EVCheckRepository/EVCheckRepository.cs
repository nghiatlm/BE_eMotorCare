using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.EVCheckRepository
{
    public class EVCheckRepository : GenericRepository<EVCheck>, IEVCheckRepository
    {
        public EVCheckRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<(IReadOnlyList<EVCheck> Items, long Total)> GetPagedAsync(
            DateTime? startDate,
            DateTime? endDate,
            EVCheckStatus? status,
            Guid? appointmentId,
            Guid? taskExecutorId,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context
                .EVChecks.Include(x => x.Appointment)
                .ThenInclude(a => a.Customer)
                .ThenInclude(a => a.Account)
                .Include(x => x.Appointment)
                .ThenInclude(a => a.ServiceCenter)
                .Include(x => x.TaskExecutor)
                .Include(x => x.EVCheckDetails)
                .ThenInclude(ms => ms.MaintenanceStageDetail)
                .ThenInclude(m => m.Part)
                .Include(x => x.EVCheckDetails)
                .ThenInclude(ms => ms.CampaignDetail)
                .Include(x => x.EVCheckDetails)
                .ThenInclude(ms => ms.PartItem)
                .Include(x => x.EVCheckDetails)
                .ThenInclude(ms => ms.ReplacePart)
                .AsNoTracking()
                .AsQueryable();

            if (status.HasValue)
            {
                q = q.Where(x => x.Status == status.Value);
            }
            if (appointmentId.HasValue)
            {
                q = q.Where(x => x.AppointmentId == appointmentId);
            }
            if (taskExecutorId.HasValue)
            {
                q = q.Where(x => x.TaskExecutorId == taskExecutorId);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                var endDateInclusive = endDate.Value.Date.AddDays(1);
                q = q.Where(x =>
                    x.CheckDate >= startDate.Value.Date && x.CheckDate < endDateInclusive
                );
            }
            else if (startDate.HasValue)
            {
                var todayEnd = DateTime.Now.Date.AddDays(1);
                q = q.Where(x => x.CheckDate >= startDate.Value.Date && x.CheckDate < todayEnd);
            }
            else if (endDate.HasValue)
            {
                var endDateInclusive = endDate.Value.Date.AddDays(1);
                q = q.Where(x => x.CheckDate < endDateInclusive);
            }

            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<EVCheck?> GetByIdAsync(Guid id)
        {
            var evCheck = await _context
                .EVChecks.Include(x => x.Appointment)
                .ThenInclude(c => c.Customer)
                .ThenInclude(a => a.Account)
                .Include(x => x.TaskExecutor)
                .Include(x => x.EVCheckDetails)
                .ThenInclude(p => p.PartItem)
                .ThenInclude(p => p.Part)
                .Include(x => x.EVCheckDetails)
                .ThenInclude(p => p.ReplacePart)
                .FirstOrDefaultAsync(x => x.Id == id);
            return evCheck;
        }

        public Task<EVCheck?> GetByAppointmentIdAsync(Guid appointmentId) =>
            _context
                .EVChecks.Include(x => x.EVCheckDetails)
                .ThenInclude(d => d.MaintenanceStageDetail)
                .Include(x => x.EVCheckDetails)
                .ThenInclude(d => d.PartItem)
                .ThenInclude(pi => pi.Part)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);

        public Task<EVCheck?> GetByIdIncludeDetailsAsync(Guid evCheckId) =>
            _context
                .EVChecks.Include(x => x.EVCheckDetails)
                .FirstOrDefaultAsync(x => x.Id == evCheckId);

        public Task<EVCheck?> GetByIdWithAppointmentAsync(Guid id) =>
            _context
                .EVChecks.AsNoTracking()
                .Include(x => x.Appointment)
                .FirstOrDefaultAsync(x => x.Id == id);
    }
}
