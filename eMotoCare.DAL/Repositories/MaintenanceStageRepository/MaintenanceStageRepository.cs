using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace eMotoCare.DAL.Repositories.MaintenanceStageRepository
{
    public class MaintenanceStageRepository : GenericRepository<MaintenanceStage>, IMaintenanceStageRepository
    {
        public MaintenanceStageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<MaintenanceStage> Items, long Total)> GetPagedAsync(
            Guid? maintenancePlanId,
            string? description,
            DurationMonth? durationMonth,
            Mileage? mileage,
            string? name,
            Status? status,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.MaintenanceStages
                .AsNoTracking()
                .Include(x => x.MaintenancePlan)
                .Include(x => x.MaintenanceStageDetails).ThenInclude(x => x.Part)
                .AsQueryable();

            if (maintenancePlanId.HasValue)
                q = q.Where(x => x.MaintenancePlanId == maintenancePlanId.Value);
            if (!string.IsNullOrWhiteSpace(description))
            {
                q = q.Where(x =>
                    x.Description.Contains(description));
            }
            if (durationMonth.HasValue)
                q = q.Where(x => x.DurationMonth == durationMonth.Value);
            if (mileage.HasValue)
                q = q.Where(x => x.Mileage == mileage.Value);
            if (!string.IsNullOrWhiteSpace(name))
            {
                q = q.Where(x =>
                    x.Name.Contains(name));
            }
            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);

            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<MaintenanceStage?> GetByIdAsync(Guid id)
        {
            return await _context
                .MaintenanceStages
                .Include(x => x.MaintenancePlan)
                .Include(x => x.MaintenanceStageDetails).ThenInclude(x => x.Part)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
