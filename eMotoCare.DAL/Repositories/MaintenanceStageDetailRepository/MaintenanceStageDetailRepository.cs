using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace eMotoCare.DAL.Repositories.MaintenanceStageDetailRepository
{
    public class MaintenanceStageDetailRepository : GenericRepository<MaintenanceStageDetail>, IMaintenanceStageDetailRepository
    {
        public MaintenanceStageDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<(IReadOnlyList<MaintenanceStageDetail> Items, long Total)> GetPagedAsync(
            Guid? maintenanceStageId,
            Guid? partId,
            ActionType[]? actionType,
            string? description,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.MaintenanceStageDetails
                .AsNoTracking()
                .Include(x => x.MaintenanceStage)
                .Include(x => x.Part)
                .AsQueryable();

            if (maintenanceStageId.HasValue)
                q = q.Where(x => x.MaintenanceStageId == maintenanceStageId.Value);
            if (partId.HasValue)
                q = q.Where(x => x.PartId == partId.Value);
            if (!string.IsNullOrWhiteSpace(description))
            {
                q = q.Where(x =>
                    x.Description.Contains(description));
            }

            if (actionType != null && actionType.Length > 0)
            {
                var unitString = string.Join(".", actionType.Select(u => u.ToString()));

                var list = await q.ToListAsync();

                list = list
                    .Where(x => string.Join(",", x.ActionType)
                        .Replace(",", ".")
                        .Contains(unitString, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                var total = list.LongCount();

                var items = list
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return (items, total);
            }
            else
            {
                var total = await q.LongCountAsync();
                var items = await q
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (items, total);
            }
        }

        public async Task<MaintenanceStageDetail?> GetByIdAsync(Guid id)
        {
            return await _context
                .MaintenanceStageDetails
                .Include(x => x.MaintenanceStage)
                .Include(x => x.Part)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
