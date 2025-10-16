using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.AccountRepository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> FindByPhone(string phone);
        Task<Account> FindByEmail(string email);
    }
}
