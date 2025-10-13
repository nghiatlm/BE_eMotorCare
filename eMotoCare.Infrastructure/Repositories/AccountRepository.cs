using eMotoCare.Application.Interfaces.IRepository;
using eMotoCare.Domain.Entities;
using eMotoCare.Infrastructure.Context;

namespace eMotoCare.Infrastructure.Repositories
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(DBContextMotoCare context) : base(context)
        {
        }

        public Task<Account> GetByPhoneAsync(string phone)
        {
            throw new NotImplementedException();
        }
    }
}
