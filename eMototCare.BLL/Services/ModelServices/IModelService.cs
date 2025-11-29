using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ModelServices
{
    public interface IModelService
    {
        Task<PageResult<ModelResponse>> GetPagedAsync(
            string? search,
            Status? status,
            Guid? modelId,
            Guid? maintenancePlanId,
            int page,
            int pageSize
        );

        Task<ModelResponse> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(ModelRequest req);
        Task UpdateAsync(Guid id, ModelUpdateRequest req);
        Task DeleteAsync(Guid id);
        Task<ModelResponse> SyncModelAsync(SyncModelRequest request);
    }
}
