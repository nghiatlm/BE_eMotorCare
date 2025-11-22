using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.CampaignRepository
{
    public class CampaignRepository : GenericRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<(IReadOnlyList<Campaign> Items, long Total)> GetPagedAsync(
            string? code,
            string? name,
            CampaignType? campaignType,
            DateTime? fromDate,
            DateTime? toDate,
            CampaignStatus? status,
            string? modelName,
            Guid? vehicleId,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);
            string? effectiveModelName = null;

            if (vehicleId.HasValue)
            {
                var vehicle = await _context
                    .Vehicles.Include(v => v.Model)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == vehicleId.Value);

                if (vehicle == null || vehicle.Model == null)
                {
                    return (Array.Empty<Campaign>(), 0);
                }

                effectiveModelName = vehicle.Model.Name?.Trim();
            }
            else if (!string.IsNullOrWhiteSpace(modelName))
            {
                effectiveModelName = modelName.Trim();
            }

            var q = _context.Campaigns.Include(x => x.CampaignDetails).AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(code))
                q = q.Where(x => x.Code.Contains(code));

            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(x => x.Name.Contains(name));

            if (campaignType.HasValue)
                q = q.Where(x => x.Type == campaignType.Value);

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);

            if (fromDate.HasValue)
                q = q.Where(x => x.EndDate >= fromDate.Value);

            if (toDate.HasValue)
                q = q.Where(x => x.StartDate <= toDate.Value);

            if (!string.IsNullOrWhiteSpace(effectiveModelName))
            {
                var m = effectiveModelName;
                q = q.Where(c => (c.ModelName != null && c.ModelName == m) || c.ModelName == null);
            }
            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.StartDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<Campaign?> GetByIdAsync(Guid id) =>
            _context.Campaigns.Include(x => x.CampaignDetails).FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.Campaigns.AnyAsync(x => x.Code == code);

        public Task<List<Campaign>> GetExpiredActiveAsync(DateTime nowLocal) =>
            _context
                .Campaigns.Where(x => x.Status == CampaignStatus.ACTIVE && x.EndDate <= nowLocal)
                .ToListAsync();
    }
}
