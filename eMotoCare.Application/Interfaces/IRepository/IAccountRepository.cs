using eMotoCare.Domain.Entities;

namespace eMotoCare.Application.Interfaces.IRepository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetByPhoneAsync(string phone);
    }
}
