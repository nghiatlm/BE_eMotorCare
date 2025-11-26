using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ModelPartTypeServices
{
    public interface IModelPartService
    {
        Task<PageResult<ModelPartResponse>> GetPagedAsync(
            string? search,
            Status? status,
            Guid? id,
            Guid? modelId,
            Guid? partId,
            int page,
            int pageSize
        );

        Task<ModelPartResponse> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(ModelPartRequest req);
        Task UpdateAsync(Guid id, ModelPartUpdateRequest req);
        Task DeleteAsync(Guid id);
    }
}
