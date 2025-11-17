using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.CampaignDetailRepository
{
    public class CampaignDetailRepository : GenericRepository<CampaignDetail>, ICampaignDetailRepository
    {
        public CampaignDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<CampaignDetail> Items, long Total)> GetPagedAsync(
            Guid? campaignId,
            Guid? partId,
            CampaignActionType? actionType,
            bool? isMandatory,
            int? estimatedTime,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.CampaignDetails
                .Include(x => x.Campaign)
                .Include(x => x.Part)
                .AsNoTracking().AsQueryable();

            if (campaignId.HasValue)
                q = q.Where(x => x.CampaignId == campaignId.Value);

            if (partId.HasValue)
                q = q.Where(x => x.PartId == partId.Value);

            if (actionType.HasValue)
                q = q.Where(x => x.ActionType == actionType.Value);

            if (isMandatory.HasValue)
                q = q.Where(x => x.IsMandatory == isMandatory.Value);

            if (estimatedTime.HasValue)
                q = q.Where(x => x.EstimatedTime == estimatedTime.Value);

            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CampaignId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<CampaignDetail?> GetByIdAsync(Guid id) =>
            _context.CampaignDetails
            .Include(x => x.Part)
            .Include(x => x.Campaign)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
