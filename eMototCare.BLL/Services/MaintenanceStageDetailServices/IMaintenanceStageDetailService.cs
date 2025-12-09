using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.MaintenanceStageDetailServices
{
    public interface IMaintenanceStageDetailService
    {
        Task<Guid> CreateAsync(MaintenanceStageDetailRequest req);
        Task DeleteAsync(Guid id);
        Task<MaintenanceStageDetailResponse?> GetByIdAsync(Guid id);
        Task<PageResult<MaintenanceStageDetailResponse>> GetPagedAsync(
            Guid? maintenanceStageId,
            Guid? partId,
            ActionType[]? actionType,
            string? description,
            int page,
            int pageSize
        );
        Task UpdateAsync(Guid id, MaintenanceStageDetailUpdateRequest req);
    }
}
