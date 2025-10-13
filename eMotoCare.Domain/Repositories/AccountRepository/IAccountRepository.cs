using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Repositories.AccountRepository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetByPhoneAsync(string phone);
    }
}
