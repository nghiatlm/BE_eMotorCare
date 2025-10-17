using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.AccountRepository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> FindByPhone(string phone);
        Task<Account> FindByEmail(string email);
        Task<(IReadOnlyList<Account> Items, long Total)> GetPagedAsync(
            string? search,
            RoleName? role,
            AccountStatus? status,
            int page,
            int pageSize
        );

        Task<bool> ExistsPhoneAsync(string phone);
        Task<bool> ExistsEmailAsync(string email);
        Task<Account?> GetByIdAsync(Guid id);
        Task<Account?> GetByPhoneAsync(string phone);
        Task<Account?> GetByEmailAsync(string email);
    }
}
