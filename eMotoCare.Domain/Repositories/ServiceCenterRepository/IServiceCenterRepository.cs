using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Repositories.ServiceCenterRepository
{
    public interface IServiceCenterRepository : IGenericRepository<ServiceCenter>
    {
        Task<(IReadOnlyList<ServiceCenter> Items, long Total)> GetPagedAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default
        );

        Task<bool> ExistsNameAsync(
            string name,
            Guid? exceptId = null,
            CancellationToken ct = default
        );
        Task<bool> ExistsPhoneAsync(
            string phone,
            Guid? exceptId = null,
            CancellationToken ct = default
        );
        Task<bool> ExistsEmailAsync(
            string email,
            Guid? exceptId = null,
            CancellationToken ct = default
        );
        Task<bool> ExistsAddressAsync(
            string address,
            Guid? exceptId = null,
            CancellationToken ct = default
        );
    }
}
