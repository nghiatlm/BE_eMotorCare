using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using eMotoCare.DAL.Repositories.BatteryCheckRepository;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.BatteryCheckRepository
{
    public class BatteryCheckRepository : GenericRepository<BatteryCheck>, IBatteryCheckRepository
    {
        public BatteryCheckRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<BatteryCheck?> GetByEVCheckDetailIdAsync(Guid evCheckDetailId)
        {
            return _context
                .BatteryChecks.AsNoTracking()
                .FirstOrDefaultAsync(x => x.EVCheckDetailId == evCheckDetailId);
        }

        public Task<BatteryCheck?> GetByIdAsync(Guid id)
        {
            return _context.BatteryChecks.AsNoTracking().Include(b => b.EVCheckDetail!).ThenInclude(d => d.EVCheck!).ThenInclude(c => c.Appointment!).ThenInclude(a => a.Vehicle).FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<(IReadOnlyList<BatteryCheck> Items, long Total)> GetPagedAsync(
            Guid? evCheckDetailId,
            DateTime? fromDate,
            DateTime? toDate,
            string? sortBy,
            bool sortDesc,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.BatteryChecks.AsNoTracking().Include(b => b.EVCheckDetail!).ThenInclude(d => d.EVCheck!).ThenInclude(c => c.Appointment!).ThenInclude(a => a.Vehicle).AsQueryable();

            if (evCheckDetailId.HasValue && evCheckDetailId.Value != Guid.Empty)
                q = q.Where(b => b.EVCheckDetailId == evCheckDetailId.Value);

            if (fromDate.HasValue)
                q = q.Where(b => b.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                q = q.Where(b => b.CreatedAt <= toDate.Value);
            sortBy = (sortBy ?? "createdAt").ToLowerInvariant();
            bool desc = sortDesc;

            q = sortBy switch
            {
                "updatedat" => desc
                    ? q.OrderByDescending(b => b.UpdatedAt)
                    : q.OrderBy(b => b.UpdatedAt),
                _ => desc ? q.OrderByDescending(b => b.CreatedAt) : q.OrderBy(b => b.CreatedAt),
            };

            var total = await q.LongCountAsync();

            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }
    }
}
