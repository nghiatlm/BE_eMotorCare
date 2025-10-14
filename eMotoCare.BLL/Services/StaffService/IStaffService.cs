using eMotoCare.Common.Enums;
using eMotoCare.Common.Models.Pages;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;

namespace eMotoCare.BLL.Services.StaffService
{
    public interface IStaffService
    {
        Task<PageResult<StaffResponse>> GetPagedAsync(
            string? search,
            Gender? gender,
            StaffPosition? position,
            Guid? branchId,
            int page,
            int pageSize,
            CancellationToken ct = default
        );

        Task<StaffResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<Guid> CreateAsync(StaffRequest req, CancellationToken ct = default);
        Task UpdateAsync(Guid id, StaffRequest req, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
