using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ServiceCenterInventoryRepository
{
    public class ServiceCenterInventoryRepository : GenericRepository<ServiceCenterInventory>, IServiceCenterInventoryRepository
    {
        public ServiceCenterInventoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<ServiceCenterInventory> Items, long Total)> GetPagedAsync(
            Guid? serviceCenterId,
            string? serviceCenterInventoryName,
            Status? status,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.ServiceCenterInventorys
                .Include(x => x.PartItems)
                .Include(s => s.ServiceCenter)
                .AsNoTracking()
                .AsQueryable();
            if (serviceCenterId.HasValue)
                q = q.Where(x => x.ServiceCenterId == serviceCenterId.Value);

            if (!string.IsNullOrWhiteSpace(serviceCenterInventoryName))
                q = q.Where(x => x.ServiceCenterInventoryName.Contains(serviceCenterInventoryName));

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);





            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.ServiceCenterInventoryName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<ServiceCenterInventory?> GetByIdAsync(Guid id) =>
        _context.ServiceCenterInventorys
            .Include(x => x.PartItems)
            .Include(s => s.ServiceCenter)
            .FirstOrDefaultAsync(x => x.Id == id);

        public Task<ServiceCenterInventory?> GetByServiceCenterId(Guid id) =>
        _context.ServiceCenterInventorys 
            .Include(x => x.PartItems) 
            .Include(s => s.ServiceCenter)
            .FirstOrDefaultAsync(x => x.ServiceCenterId == id);
    }
}
