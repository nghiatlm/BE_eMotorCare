using eMotoCare.Common.Models.Pages;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;

namespace eMotoCare.BLL.Services.ServiceCenterServices
{
    public interface IServiceCenterService
    {
        Task<PageResult<ServiceCenterResponse>> GetPagedAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default
        );
        Task<ServiceCenterResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<Guid> CreateAsync(ServiceCenterRequest req, CancellationToken ct = default);
        Task UpdateAsync(Guid id, ServiceCenterRequest req, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
