using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.AccountRepository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {

        public AccountRepository(ApplicationDbContext context) : base(context) { }
    }
}
