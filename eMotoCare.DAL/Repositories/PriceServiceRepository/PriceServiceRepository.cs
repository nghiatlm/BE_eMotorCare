using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.PriceServiceRepository
{
    public class PriceServiceRepository : GenericRepository<PriceService>, IPriceServiceRepository
    {
        public PriceServiceRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<PriceService?> GetByIdAsync(Guid id) =>
            _context
                .PriceServices.AsNoTracking()
                .Include(x => x.PartType)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(IReadOnlyList<PriceService> Items, long Total)> GetPagedAsync(
            string? search,
            Guid? partTypeId,
            Remedies? remedies,
            DateTime? fromEffectiveDate,
            DateTime? toEffectiveDate,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.PriceServices.AsNoTracking().Include(x => x.PartType).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    x.Code.ToLower().Contains(s)
                    || x.Name.ToLower().Contains(s)
                    || (x.Description != null && x.Description.ToLower().Contains(s))
                );
            }

            if (partTypeId.HasValue)
                q = q.Where(x => x.PartTypeId == partTypeId.Value);
            if (remedies.HasValue)
                q = q.Where(x => x.Remedies == remedies.Value);
            if (fromEffectiveDate.HasValue)
                q = q.Where(x => x.EffectiveDate.Date >= fromEffectiveDate.Value.Date);
            if (toEffectiveDate.HasValue)
                q = q.Where(x => x.EffectiveDate.Date <= toEffectiveDate.Value.Date);
            if (minPrice.HasValue)
                q = q.Where(x => x.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                q = q.Where(x => x.Price <= maxPrice.Value);

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.EffectiveDate)
                .ThenBy(x => x.Code)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<bool> ExistsCodeAsync(string code, Guid? ignoreId = null)
        {
            var q = _context.PriceServices.AsQueryable().Where(x => x.Code == code);
            if (ignoreId.HasValue)
                q = q.Where(x => x.Id != ignoreId.Value);
            return q.AnyAsync();
        }
    }
}
