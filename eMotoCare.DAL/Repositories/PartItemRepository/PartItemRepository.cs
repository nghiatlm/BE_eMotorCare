using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.PartItemRepository
{
    public class PartItemRepository : GenericRepository<PartItem>, IPartItemRepository
    {
        public PartItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<PartItem> Items, long Total)> GetPagedAsync(
             Guid? partId,
             string? serialNumber,
             PartItemStatus? status,
             Guid? serviceCenterInventoryId,
             int page,
             int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.PartItems
                .Include(x => x.Part)
                .AsNoTracking()
                .AsQueryable();
            
            if (partId.HasValue)
                q = q.Where(x => x.PartId == partId.Value);
            if (!string.IsNullOrWhiteSpace(serialNumber))
                q = q.Where(x => x.SerialNumber.Contains(serialNumber));
            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (serviceCenterInventoryId.HasValue)
                q = q.Where(x => x.ServiceCenterInventoryId == serviceCenterInventoryId);



            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<PartItem?> GetByIdAsync(Guid id) =>
        _context.PartItems.FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsSerialNumberAsync(string serialNumber) =>
            _context.PartItems.AnyAsync(x => x.SerialNumber == serialNumber);

   

        public async Task<List<PartItem>> GetByServiceCenterIdAsync(Guid serviceCenterId)
        {
            return await _context.PartItems
                .Include(p => p.ServiceCenterInventory)
                .Include(p => p.Part)
                .Where(p => p.ServiceCenterInventory.ServiceCenterId == serviceCenterId)
                .ToListAsync();
        }
    }
}
