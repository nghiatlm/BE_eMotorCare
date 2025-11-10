using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.CustomerRepository
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByIdAsync(Guid id);
        Task<bool> ExistsCitizenAsync(string citizenId, Guid? excludeCustomerId = null);
        Task<bool> ExistsForAccountAsync(Guid accountId);
        Task<IReadOnlyList<Customer>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds);
        Task<Customer?> GetAccountIdAsync(Guid id);
        Task<(IReadOnlyList<Customer> Items, long Total)> GetPagedAsync(
            string? search,
            int page,
            int pageSize
        );
        Task<Customer?> GetByCitizenId(string citizenId);
    }
}
