

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ServiceCenterInventoryServices
{
    public interface IServiceCenterInventoryService
    {
        Task<Guid> CreateAsync(ServiceCenterInventoryRequest req);
        Task DeleteAsync(Guid id);
        Task<ServiceCenterInventoryResponse?> GetByIdAsync(Guid id);
        Task<PageResult<ServiceCenterInventoryResponse>> GetPagedAsync(Guid? serviceCenterId, string? serviceCenterInventoryName, Status? status, int page, int pageSize);
        Task UpdateAsync(Guid id, ServiceCenterInventoryUpdateRequest req);
    }
}
