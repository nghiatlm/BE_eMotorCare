using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.VehicleStageServices
{
    public interface IVehicleStageService
    {
        Task<PageResult<VehicleStageResponse>> GetPagedAsync(
            Guid? vehicleId,
            Guid? maintenanceStageId,
            VehicleStageStatus? status,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        );

        Task<VehicleStageResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(VehicleStageRequest req);
        Task UpdateAsync(Guid id, VehicleStageRequest req);
        Task DeleteAsync(Guid id);
    }
}
