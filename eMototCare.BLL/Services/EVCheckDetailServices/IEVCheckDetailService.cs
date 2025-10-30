
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.EVCheckDetailServices
{
    public interface IEVCheckDetailService
    {
        Task<Guid> CreateAsync(EVCheckDetailRequest req);
        Task DeleteAsync(Guid id);
        Task<EVCheckDetailResponse?> GetByIdAsync(Guid id);
        Task<PageResult<EVCheckDetailResponse>> GetPagedAsync(Guid? maintenanceStageDetailId, Guid? campaignDetailId, Guid? partItemId, Guid? eVCheckId, Guid? replacePartId, string? result, Remedies? remedies, string? unit, decimal? quantity, decimal? pricePart, decimal? priceService, decimal? totalAmount, EVCheckDetailStatus? status, int page, int pageSize);
        Task UpdateAsync(Guid id, EVCheckDetailUpdateRequest req);
    }
}
