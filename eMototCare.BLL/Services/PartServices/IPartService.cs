

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
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
        Task<PageResult<PartResponse>> GetPagedAsync(Guid? partTypeId, string? code, string? name, PartStatus? status, int? quantity, int page = 1, int pageSize = 10);
        Task UpdateAsync(Guid id, PartRequest req);
    }
}
