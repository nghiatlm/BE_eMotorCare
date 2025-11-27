using eMotoCare.BO.Entities;
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
             Guid? serviceCenterId,
             int page,
             int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.PartItems
                .Include(x => x.Part)
                .ThenInclude(x => x.PartType)
                .Include(x => x.ServiceCenterInventory)
                .AsNoTracking()
                .AsQueryable();

            if (partId.HasValue)
                q = q.Where(x => x.PartId == partId.Value);
            if (!string.IsNullOrWhiteSpace(serialNumber))
                q = q.Where(x => x.SerialNumber.Contains(serialNumber));
            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (serviceCenterId.HasValue)
                q = q.Where(x => x.ServiceCenterInventory != null && x.ServiceCenterInventory.ServiceCenterId == serviceCenterId.Value);
            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<PartItem?> GetByIdAsync(Guid id) =>
        _context.PartItems
            .Include(x => x.ServiceCenterInventory)
                .ThenInclude(x => x.ServiceCenter)
            .Include(x => x.Part)
            .ThenInclude(x => x.PartType)
            .Include(x => x.ExportNoteDetails)
            .FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsSerialNumberAsync(string serialNumber) =>
            _context.PartItems.AnyAsync(x => x.SerialNumber == serialNumber);


        // fix lại, đg sửa tạm
        public Task<List<PartItem>> GetByExportNoteIdAsync(Guid exportNoteId) =>
            _context.PartItems
            .Include(x => x.Part)
            .Where(x => x.ServiceCenterInventoryId == exportNoteId)
            .ToListAsync();

        public async Task<List<PartItem>> GetByServiceCenterIdAsync(Guid serviceCenterId)
        {
            return await _context.PartItems
                .Include(p => p.ServiceCenterInventory)
                .Include(p => p.Part)
                .Where(p => p.ServiceCenterInventory.ServiceCenterId == serviceCenterId)
                .ToListAsync();
        }

        public async Task<List<PartItem>> GetAvailablePartItemsByPartIdAsync(Guid partId, Guid serviceCenterId)
        {
            return await _context.PartItems
                .Include(p => p.ServiceCenterInventory)
                .Include(p => p.Part)
                .Where(p => p.PartId == partId && p.ServiceCenterInventory.ServiceCenterId == serviceCenterId && p.Quantity == 1 && p.Status == PartItemStatus.ACTIVE)
                .ToListAsync();
        }
    }
}
