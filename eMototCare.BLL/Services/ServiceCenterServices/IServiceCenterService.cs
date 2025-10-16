using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ServiceCenterServices
{
    public interface IServiceCenterService
    {
        Task<PageResult<ServiceCenterResponse>> GetPagedAsync(
            string? search,
            StatusEnum? status,
            int page,
            int pageSize
        );

        Task<ServiceCenterResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(ServiceCenterRequest req);
        Task UpdateAsync(Guid id, ServiceCenterRequest req);
        Task DeleteAsync(Guid id);
    }
}
