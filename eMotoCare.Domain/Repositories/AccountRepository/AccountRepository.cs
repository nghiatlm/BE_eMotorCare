using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Entities;
using eMotoCare.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.AccountRepository
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
