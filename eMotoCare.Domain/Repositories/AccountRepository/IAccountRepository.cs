using eMotoCare.Common.Enums;
using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Repositories.AccountRepository
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account> GetByPhoneAsync(string phone);
        Task<Account> GetByEmailAsync(string email);

        Task<(IReadOnlyList<Account> Items, long Total)> GetPagedAsync(
            string? search,
            RoleName? role,
            AccountStatus? status,
            int page,
            int pageSize
        );
        Task<bool> ExistsPhoneAsync(string phone);
        Task<bool> ExistsEmailAsync(string email);
    }
}
