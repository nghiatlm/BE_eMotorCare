using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.CampaignServices
{
    public interface ICampaignService
    {
        Task<Guid> CreateAsync(CampaignRequest req);
        Task DeleteAsync(Guid id);
        Task<CampaignResponse?> GetByIdAsync(Guid id);
        Task<PageResult<CampaignResponse>> GetPagedAsync(
            string? code,
            string? name,
            CampaignType? campaignType,
            DateTime? fromDate,
            DateTime? toDate,
            CampaignStatus? status,
            string? modelName,
            Guid? vehicleId,
            int page,
            int pageSize
        );
        Task UpdateAsync(Guid id, CampaignUpdateRequest req);
    }
}
