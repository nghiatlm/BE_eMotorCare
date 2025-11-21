

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.CampaignDetailServices
{
    public interface ICampaignDetailService
    {
        Task<Guid> CreateAsync(CampaignDetailRequest req);
        Task DeleteAsync(Guid id);
        Task<CampaignDetailResponse?> GetByIdAsync(Guid id);
        Task<PageResult<CampaignDetailResponse>> GetPagedAsync(Guid? campaignId, Guid? partId, CampaignActionType? actionType, bool? isMandatory, int? estimatedTime, int page, int pageSize);
        Task UpdateAsync(Guid id, CampaignDetailUpdateRequest req);
    }
}
