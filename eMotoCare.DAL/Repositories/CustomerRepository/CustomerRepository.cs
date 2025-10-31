using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.CustomerRepository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<Customer?> GetByIdAsync(Guid id) =>
            _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public Task<Customer?> GetByAccountIdAsync(Guid accountId) =>
            _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.AccountId == accountId);

        public Task<bool> ExistsCitizenAsync(string citizenId, Guid? excludeCustomerId = null)
        {
            var q = _context.Customers.AsQueryable().Where(x => x.CitizenId == citizenId);
            if (excludeCustomerId.HasValue)
                q = q.Where(x => x.Id != excludeCustomerId.Value);
            return q.AnyAsync();
        }

        public Task<bool> ExistsForAccountAsync(Guid accountId) =>
            _context.Customers.AnyAsync(x => x.AccountId == accountId);

        public async Task<(IReadOnlyList<Customer> Items, long Total)> GetPagedAsync(
            string? search,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Customers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    (x.FirstName != null && x.FirstName.ToLower().Contains(s))
                    || (x.LastName != null && x.LastName.ToLower().Contains(s))
                    || x.CitizenId.ToLower().Contains(s)
                );
            }

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<IReadOnlyList<Customer>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds) =>
            _context
                .Customers.AsNoTracking()
                .Where(c => accountIds.Contains(c.AccountId))
                .ToListAsync()
                .ContinueWith(task => (IReadOnlyList<Customer>)task.Result);
    }
}
