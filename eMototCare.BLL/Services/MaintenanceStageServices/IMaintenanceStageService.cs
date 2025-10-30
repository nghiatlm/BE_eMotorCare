
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.MaintenanceStageServices
{
    public interface IMaintenanceStageService
    {
        Task<Guid> CreateAsync(MaintenanceStageRequest req);
        Task DeleteAsync(Guid id);
        Task<MaintenanceStageResponse?> GetByIdAsync(Guid id);
        Task<PageResult<MaintenanceStageResponse>> GetPagedAsync(Guid? maintenancePlanId, string? description, DurationMonth? durationMonth, Mileage? mileage, string? name, Status? status, int page = 1, int pageSize = 10);
        Task UpdateAsync(Guid id, MaintenanceStageUpdateRequest req);
    }
}
