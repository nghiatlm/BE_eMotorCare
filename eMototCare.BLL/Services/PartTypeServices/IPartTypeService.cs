


using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.DTO.Responses.Labels;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.PartTypeServices
{
    public interface IPartTypeService
    {
        Task<Guid> CreateAsync(PartTypeRequest req);
        Task DeleteAsync(Guid id);
        Task<PartTypeResponse?> GetByIdAsync(Guid id);
        Task<PageResult<PartTypeResponse>> GetPagedAsync(string? name, string? description, int page, int pageSize);
        Task UpdateAsync(Guid id, PartTypeUpdateRequest req);
        Task<List<eMotoCare.BO.DTO.Responses.Labels.PartType>> GetAll();
    }
}
