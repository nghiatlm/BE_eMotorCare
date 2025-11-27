using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.VehiclePartItemRepository
{
    public class VehiclePartItemRepository
        : GenericRepository<VehiclePartItem>,
            IVehiclePartItemRepository
    {
        public VehiclePartItemRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<VehiclePartItem?> GetByIdAsync(Guid id) =>
            _context
                .VehiclePartItems.AsNoTracking()
                .Include(x => x.Vehicle)
                .Include(x => x.PartItem)
                .ThenInclude(x => x.Part)
                .Include(x => x.ReplaceFor)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(IReadOnlyList<VehiclePartItem> Items, long Total)> GetPagedAsync(
            string? search,
            Guid? vehicleId,
            Guid? partItemId,
            Guid? replaceForId,
            DateTime? fromInstallDate,
            DateTime? toInstallDate,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context
                .VehiclePartItems.AsNoTracking()
                .Include(x => x.Vehicle)
                .Include(x => x.PartItem)
                .ThenInclude(x => x.Part)
                .ThenInclude(x => x.PartType)
                .Include(x => x.ReplaceFor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    (
                        x.PartItemId.ToString().ToLower().Contains(s)
                        || (
                            x.ReplaceForId.HasValue
                            && x.ReplaceForId.ToString()!.ToLower().Contains(s)
                        )
                    )
                );
            }

            if (vehicleId.HasValue)
                q = q.Where(x => x.VehicleId == vehicleId.Value);
            if (partItemId.HasValue)
                q = q.Where(x => x.PartItemId == partItemId.Value);
            if (replaceForId.HasValue)
                q = q.Where(x => x.ReplaceForId == replaceForId.Value);
            if (fromInstallDate.HasValue)
                q = q.Where(x => x.InstallDate.Date >= fromInstallDate.Value.Date);
            if (toInstallDate.HasValue)
                q = q.Where(x => x.InstallDate.Date <= toInstallDate.Value.Date);

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.InstallDate)
                .ThenByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<List<VehiclePartItem>> GetListByVehicleIdAsync(Guid id)
        {
            return await _context
                .VehiclePartItems.AsNoTracking()
                .Include(x => x.PartItem)
                .ThenInclude(x => x.Part)
                .Where(x => x.VehicleId == id)
                .ToListAsync();
        }
    }
}
