using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ModelPartTypeServices
{
    public interface IModelPartTypeService
    {
        Task<PageResult<ModelPartTypeResponse>> GetPagedAsync(
            string? search,
            Status? status,
            Guid? id,
            Guid? modelId,
            Guid? partTypeId,
            int page,
            int pageSize
        );

        Task<ModelPartTypeResponse> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(ModelPartTypeRequest req);
        Task UpdateAsync(Guid id, ModelPartTypeUpdateRequest req);
        Task DeleteAsync(Guid id);
    }
}
