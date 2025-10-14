using eMotoCare.Common.Enums;
using eMotoCare.Common.Models.Pages;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;

namespace eMotoCare.BLL.Services.BranchServices
{
    public interface IBranchService
    {
        Task<PageResult<BranchResponse>> GetPagedAsync(
            string? search,
            Status? status,
            int page,
            int pageSize,
            CancellationToken ct = default
        );
        Task<BranchResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<Guid> CreateAsync(BranchRequest req, CancellationToken ct = default);
        Task UpdateAsync(Guid id, BranchRequest req, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
