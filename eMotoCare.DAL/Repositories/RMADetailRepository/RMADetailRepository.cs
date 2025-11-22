using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.RMADetailRepository
{
    public class RMADetailRepository : GenericRepository<RMADetail>, IRMADetailRepository
    {
        public RMADetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<RMADetail> Items, long Total)> GetPagedAsync(
             string? rmaNumber,
             string? inspector,
             string? result,
             string? solution,
             Guid? evCheckDetailId,
             Guid? rmaId,
             RMADetailStatus? status,
             int page,
             int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.RMADetails
                .Include(e => e.EVCheckDetail)
                    .ThenInclude(e => e.PartItem)
                        .ThenInclude(p => p.Part)
                .Include(r => r.RMA)
                .AsNoTracking()
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(rmaNumber))
                q = q.Where(x => x.RMANumber.Contains(rmaNumber));

            if (!string.IsNullOrWhiteSpace(inspector))
                q = q.Where(x => x.Inspector.Contains(inspector));

            if (!string.IsNullOrWhiteSpace(result))
                q = q.Where(x => x.Result.Contains(result));

            if (!string.IsNullOrWhiteSpace(solution))
                q = q.Where(x => x.Solution.Contains(solution));

            if (evCheckDetailId.HasValue)
                q = q.Where(x => x.EVCheckDetailId == evCheckDetailId.Value);

            if (rmaId.HasValue)
                q = q.Where(x => x.RMAId == rmaId.Value);

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);


            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<RMADetail?> GetByIdAsync(Guid id) =>
        _context.RMADetails
            .Include(e => e.EVCheckDetail)
                .ThenInclude(e => e.PartItem)
                        .ThenInclude(p => p.Part)
            .Include(r => r.RMA)
            .Include(e => e.EVCheckDetail)
                .ThenInclude(ev => ev.EVCheck)
                    .ThenInclude(x => x.Appointment)
            .FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsRmaNumberAsync(string rmaNumber) =>
            _context.RMADetails.AnyAsync(x => x.RMANumber == rmaNumber);

        public Task<List<RMADetail?>> GetByRmaId(Guid id)
        {
            return _context.RMADetails.Where(x => x.RMAId == id).ToListAsync();
        }
    }
}
