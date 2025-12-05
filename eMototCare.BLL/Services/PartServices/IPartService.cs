

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.DTO.Responses.Labels;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.PartServices
{
    public interface IPartService
    {
        Task<Guid> CreateAsync(PartRequest req);
        Task DeleteAsync(Guid id);
        Task<PartResponse?> GetByIdAsync(Guid id);
        Task<PageResult<PartResponse>> GetPagedAsync(Guid? partTypeId, string? code, string? name, Status? status, int? quantity, Guid? serviceCenterId, int page, int pageSize);
        Task UpdateAsync(Guid id, PartUpdateRequest req);
        Task<List<PartLabel>> GetByPartType(Guid partTypeId);
        Task<List<PartLabel>> GetPartsByModelandType(Guid modelId, Guid partTypeId);
    }
}
