using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.EVCheckDetailRepository
{
    public class EVCheckDetailRepository : GenericRepository<EVCheckDetail>, IEVCheckDetailRepository
    {
        public EVCheckDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<EVCheckDetail> Items, long Total)> GetPagedAsync(
             Guid? maintenanceStageDetailId,
             Guid? campaignDetailId,
             Guid? partItemId,
             Guid? eVCheckId,
             Guid? replacePartId,
             string? result,
             string? unit,
             decimal? quantity,
             decimal? pricePart,
             decimal? priceService,
             decimal? totalAmount,
             EVCheckDetailStatus? status,
             int page,
             int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.EVCheckDetails
                .Include(x => x.MaintenanceStageDetail)
                .Include(x => x.CampaignDetail)
                .Include(x => x.PartItem)
                .Include(x => x.PartItem)
                .Include(x => x.PartItem)
                    .ThenInclude(p => p.Part)
                .Include(x => x.EVCheck)
                    .ThenInclude(x => x.Appointment)
                .Include(x => x.ReplacePart)
                .Include(x => x.ReplacePart)
                .Include(x => x.ReplacePart)
                    .ThenInclude(r => r.Part)
                .AsNoTracking()
                .AsQueryable();
            if (maintenanceStageDetailId.HasValue)
            {
                q = q.Where(x => x.MaintenanceStageDetailId == maintenanceStageDetailId.Value);
            }

            if (campaignDetailId.HasValue) 
            {
                q = q.Where(x => x.CampaignDetailId == campaignDetailId.Value);
            }

            if (partItemId.HasValue)
            {
                q = q.Where(x => x.PartItemId == partItemId.Value);
            }

            if (eVCheckId.HasValue)
            {
                q = q.Where(x => x.EVCheckId == eVCheckId.Value);
            }

            if (replacePartId.HasValue)
            {
                q = q.Where(x => x.ReplacePartId == replacePartId.Value);
            }

            if (!string.IsNullOrWhiteSpace(result))
            {
                q = q.Where(x =>
                    x.Result.Contains(result));
            }

            if (!string.IsNullOrWhiteSpace(unit))
            {
                q = q.Where(x =>
                    x.Unit.Contains(unit));
            }

            if (quantity.HasValue)
            {
                q = q.Where(x => x.Quantity == quantity.Value);
            }

            if (pricePart.HasValue)
            {
                q = q.Where(x => x.PricePart == pricePart.Value);
            }

            if (priceService.HasValue)
            {
                q = q.Where(x => x.PriceService == priceService);
            }

            if (totalAmount.HasValue)
            {
                q = q.Where(x => x.TotalAmount == totalAmount);
            }

            if (status.HasValue)
            {
                q = q.Where(x => x.Status == status.Value);
            }





            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.EVCheck.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<EVCheckDetail?> GetByIdAsync(Guid id) =>
            _context.EVCheckDetails
            .Include(x => x.MaintenanceStageDetail)
                .Include(x => x.CampaignDetail)
                .Include(x => x.PartItem)
                .Include(x => x.PartItem)
                .Include(x => x.PartItem)
                    .ThenInclude(p => p.Part)
                .Include(x => x.EVCheck)
                    .ThenInclude(x => x.Appointment)
                        .ThenInclude(a => a.VehicleStage)
                .Include(x => x.ReplacePart)
                .Include(x => x.ReplacePart)
                .Include(x => x.ReplacePart)
                    .ThenInclude(r => r.Part)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
