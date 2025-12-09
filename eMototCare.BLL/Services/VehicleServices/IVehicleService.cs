using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.VehicleServices
{
    public interface IVehicleService
    {
        Task<PageResult<VehicleResponse>> GetPagedAsync(
            string? search,
            StatusEnum? status,
            Guid? modelId,
            Guid? customerId,
            DateTime? fromPurchaseDate,
            DateTime? toPurchaseDate,
            int page,
            int pageSize
        );

        Task<VehicleResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(VehicleRequest req);
        Task UpdateAsync(Guid id, VehicleRequest req);
        Task DeleteAsync(Guid id);
        Task<VehicleHistoryResponse> GetHistoryAsync(Guid vehicleId);
        Task<List<VehicleResponse>> SyncVehicleAsync();
    }
}
