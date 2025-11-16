using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.RMARepository
{
    public class RMARepository : GenericRepository<RMA>, IRMARepository
    {
        public RMARepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<RMA> Items, long Total)> GetPagedAsync(
             string? code,
             DateTime? fromDate,
             DateTime? toDate,
             string? returnAddress,
             RMAStatus? status,
             Guid? createdById,
             int page,
             int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.RMAs
                .Include(x => x.Staff)
                .Include(x => x.RMADetails)
                    .ThenInclude(x => x.EVCheckDetail)
                        .ThenInclude(x => x.PartItem)
                .AsNoTracking()
                .AsQueryable();
            

            if (!string.IsNullOrWhiteSpace(code))
                q = q.Where(x => x.Code.Contains(code));

            if (!string.IsNullOrWhiteSpace(returnAddress))
                q = q.Where(x => x.ReturnAddress.Contains(returnAddress));

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);

            if (createdById.HasValue)
                q = q.Where(x => x.CreateById == createdById.Value);


            if (fromDate.HasValue && toDate.HasValue)
            {
                var endDateInclusive = toDate.Value.Date.AddDays(1);
                q = q.Where(x =>
                    x.RMADate >= fromDate.Value.Date && x.RMADate < endDateInclusive
                );
            }
            else if (fromDate.HasValue)
            {
                var todayEnd = DateTime.Now.Date.AddDays(1);
                q = q.Where(x => x.RMADate >= fromDate.Value.Date && x.RMADate < todayEnd);
            }
            else if (toDate.HasValue)
            {
                var endDateInclusive = toDate.Value.Date.AddDays(1);
                q = q.Where(x => x.RMADate < endDateInclusive);
            }



            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<RMA?> GetByIdAsync(Guid id) =>
        _context.RMAs
            .Include(x => x.Staff)
            .Include(x => x.RMADetails)
                .ThenInclude(x => x.EVCheckDetail)
                    .ThenInclude (x => x.PartItem)
            .FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.RMAs.AnyAsync(x => x.Code == code);

        public async Task<List<RMA?>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Appointments
                        .Where(a => a.CustomerId == customerId)
                        .Select(a => a.EVCheck)                     // 1-1
                        .Where(ev => ev != null)
                        .SelectMany(ev => ev.EVCheckDetails)        // 1-N
                        .Select(ecd => ecd.RMADetail)               // 1-1
                        .Where(rmad => rmad != null)
                        .Select(rmad => rmad.RMA)                   // N-1
                        .Where(rma => rma != null)
                        .Distinct()
                        .ToListAsync();
        }

    }
}
