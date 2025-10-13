using eMotoCare.Application.Interfaces.IRepository;
using eMotoCare.Domain.Entities;
using eMotoCare.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(DBContextMotoCare context) : base(context)
        {
        }

        public async Task<Account> GetByPhoneAsync(string phone)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Phone == phone);
        }
    }
}
