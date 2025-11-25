using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ModelRepository
{
    public class ModelRepository : GenericRepository<Model>, IModelRepository
    {
        public ModelRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<Model?> GetByIdAsync(Guid id) =>
            await _context
                .Models.Include(x => x.MaintenancePlan)
                .Include(x => x.Vehicles)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(IReadOnlyList<Model> Items, long Total)> GetPagedAsync(
            string? search,
            Status? status,
            Guid? modelId,
            Guid? maintenancePlanId,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context
                .Models.AsNoTracking()
                .Include(m => m.MaintenancePlan)
                .Include(m => m.Vehicles)
                .Include(m => m.ModelPartTypes)
                .AsQueryable();
            if (modelId.HasValue && modelId.Value != Guid.Empty)
                q = q.Where(m => m.Id == modelId.Value);
            if (maintenancePlanId.HasValue && maintenancePlanId.Value != Guid.Empty)
                q = q.Where(m => m.MaintenancePlanId == maintenancePlanId.Value);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(m =>
                    m.Code.ToLower().Contains(s)
                    || m.Name.ToLower().Contains(s)
                    || m.Manufacturer.ToLower().Contains(s)
                );
            }

            if (status.HasValue)
                q = q.Where(m => m.Status == status.Value);

            var total = await q.LongCountAsync();

            var items = await q.OrderBy(m => m.Name)
                .ThenBy(m => m.Code)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.Models.AnyAsync(m => m.Code == code);

        public Task<bool> ExistsNameAsync(string name, Guid? ignoreId = null)
        {
            var q = _context.Models.AsQueryable().Where(m => m.Name == name);

            if (ignoreId.HasValue)
                q = q.Where(m => m.Id != ignoreId.Value);

            return q.AnyAsync();
        }
    }
}
