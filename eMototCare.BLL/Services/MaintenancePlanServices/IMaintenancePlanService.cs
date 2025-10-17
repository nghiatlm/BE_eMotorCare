

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.MaintenancePlanServices
{
    public interface IMaintenancePlanService
    {
        Task<Guid> CreateAsync(MaintenancePlanRequest req);
        Task DeleteAsync(Guid id);
        Task<MaintenancePlanResponse?> GetByIdAsync(Guid id);
        Task<PageResult<MaintenancePlanResponse>> GetPagedAsync(string? code, string? description, string? name, int? totalStage, Status? status, MaintenanceUnit? maintenanceUnit, int page = 1, int pageSize = 10);
        Task UpdateAsync(Guid id, MaintenancePlanRequest req);
    }
}
