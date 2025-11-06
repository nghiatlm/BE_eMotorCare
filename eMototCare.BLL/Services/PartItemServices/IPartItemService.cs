

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.PartItemServices
{
    public interface IPartItemService
    {
        Task<Guid> CreateAsync(PartItemRequest req);
        Task DeleteAsync(Guid id);
        Task<PartItemResponse?> GetByIdAsync(Guid id);
        Task<PageResult<PartItemResponse>> GetPagedAsync(Guid? partId, string? serialNumber, PartItemStatus? status, Guid? serviceCenterInventoryId, int page, int pageSize);
        Task UpdateAsync(Guid id, PartItemUpdateRequest req);
    }
}
