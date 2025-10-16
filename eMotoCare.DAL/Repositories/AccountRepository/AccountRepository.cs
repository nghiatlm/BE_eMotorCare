using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.AccountRepository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {

        public AccountRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Account?> FindByEmail(string email)
        {
            return await _context.Accounts.FindAsync(email);
        }

        public async Task<Account?> FindByPhone(string phone)
        {
            return await _context.Accounts.FindAsync(phone);
        }
    }
}
