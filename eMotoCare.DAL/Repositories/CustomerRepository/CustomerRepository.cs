using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.CustomerRepository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context
                .Customers
                .Include(x => x.Account)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<bool> ExistsCitizenAsync(string citizenId) =>
            _context.Customers.AnyAsync(x => x.CitizenId == citizenId);

        public async Task<(IReadOnlyList<Customer> Items, long Total)> GetPagedAsync(
            string? firstName,
            string? lastName,
            string? address,
            string? citizenId,
            Guid? accountId,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Customers
                .AsNoTracking()
                .Include(x => x.Account)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                q = q.Where(x =>
                    x.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                q = q.Where(x =>
                    x.LastName.Contains(lastName));
            }
            if (!string.IsNullOrWhiteSpace(address))
            {
                q = q.Where(x =>
                    x.Address.Contains(address));
            }
            if (!string.IsNullOrWhiteSpace(citizenId))
            {
                q = q.Where(x =>
                    x.CitizenId.Contains(citizenId));
            }
            if (accountId.HasValue)
                q = q.Where(x => x.AccountId == accountId.Value);




            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
