using eMotoCare.Common.Enums;
using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Context;
using eMotoCare.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.AccountRepository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(DBContextMotoCare context)
            : base(context) { }

        public async Task<Account> GetByPhoneAsync(string phone)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Phone == phone);
        }

        public async Task<Account> GetByEmailAsync(string email)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<(IReadOnlyList<Account> Items, long Total)> GetPagedAsync(
            string? search,
            RoleName? role,
            AccountStatus? status,
            int page,
            int pageSize
        )
        {
            var q = _context.Accounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(a =>
                    a.Phone.Contains(search) || (a.Email != null && a.Email.Contains(search))
                //|| (a.FullName != null && a.FullName.Contains(search))
                );
            }
            if (role.HasValue)
                q = q.Where(a => a.Role == role.Value);
            if (status.HasValue)
                q = q.Where(a => a.AccountStatus == status.Value);

            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<bool> ExistsPhoneAsync(string phone) =>
            _context.Accounts.AnyAsync(a => a.Phone == phone);

        public Task<bool> ExistsEmailAsync(string email) =>
            _context.Accounts.AnyAsync(a => a.Email == email);
    }
}
