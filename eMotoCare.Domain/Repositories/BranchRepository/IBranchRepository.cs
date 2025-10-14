using eMotoCare.Common.Enums;
using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Repositories.BranchRepository
{
    public interface IBranchRepository : IGenericRepository<Branch>
    {
        Task<(IReadOnlyList<Branch> Items, long Total)> GetPagedAsync(
            string? search,
            Status? status,
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
