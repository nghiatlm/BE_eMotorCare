using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.CustomerRepository
{
    public interface ICustomerRepository : IGenericRepository<Customer> {

        Task<Customer?> GetByIdAsync(Guid id);
        Task<bool> ExistsCitizenAsync(string citizenId);
        Task<(IReadOnlyList<Customer> Items, long Total)> GetPagedAsync(
            string? firstName,
            string? lastName,
            string? address,
            string? citizenId,
            Guid? accountId,
            int page,
            int pageSize
        );
    }
}
