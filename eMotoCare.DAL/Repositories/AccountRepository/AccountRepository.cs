using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.AccountRepository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<Account?> FindByEmail(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<Account?> FindByPhone(string phone)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Phone.Equals(phone));
        }

        public Task<Account?> GetByIdAsync(Guid id) =>
            _context.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public Task<Account?> GetByPhoneAsync(string phone) =>
            _context.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Phone == phone);

        public Task<Account?> GetByEmailAsync(string email) =>
            _context.Accounts.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        public Task<bool> ExistsPhoneAsync(string phone) =>
            _context.Accounts.AnyAsync(x => x.Phone == phone);

        public Task<bool> ExistsEmailAsync(string email) =>
            _context.Accounts.AnyAsync(x => x.Email == email);

        public async Task<(IReadOnlyList<Account> Items, long Total)> GetPagedAsync(
            string? search,
            RoleName? role,
            AccountStatus? status,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Accounts.AsNoTracking().AsQueryable();
            if (!role.HasValue)
                q = q.Where(x => x.RoleName != RoleName.ROLE_ADMIN);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    x.Phone.ToLower().Contains(s)
                    || (x.Email != null && x.Email.ToLower().Contains(s))
                );
            }

            if (role.HasValue)
                q = q.Where(x => x.RoleName == role.Value);
            if (status.HasValue)
                q = q.Where(x => x.Stattus == status.Value);

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<Staff?> GetByAccountIdAsync(Guid accountId) =>
            _context.Staffs.AsNoTracking().FirstOrDefaultAsync(s => s.AccountId == accountId);
    }
}
