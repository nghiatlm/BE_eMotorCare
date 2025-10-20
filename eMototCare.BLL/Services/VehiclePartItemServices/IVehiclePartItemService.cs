using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.VehiclePartItemServices
{
    public interface IVehiclePartItemService
    {
        Task<PageResult<VehiclePartItemResponse>> GetPagedAsync(
            string? search,
            Guid? vehicleId,
            Guid? partItemId,
            Guid? replaceForId,
            DateTime? fromInstallDate,
            DateTime? toInstallDate,
            int page,
            int pageSize
        );

        Task<VehiclePartItemResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(VehiclePartItemRequest req);
        Task UpdateAsync(Guid id, VehiclePartItemRequest req);
        Task DeleteAsync(Guid id);
    }
}
