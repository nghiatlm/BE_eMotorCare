
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.RMADetailServices
{
    public interface IRMADetailService
    {
        Task<Guid> CreateAsync(RMADetailRequest req);
        Task DeleteAsync(Guid id);
        Task<RMADetailResponse?> GetByIdAsync(Guid id);
        Task<PageResult<RMADetailResponse>> GetPagedAsync(string? rmaNumber, string? inspector, string? result, string? solution, Guid? evCheckDetailId, Guid? rmaId, RMADetailStatus? status, int page, int pageSize);
        Task UpdateAsync(Guid id, RMADetailUpdateRequest req);
    }
}
