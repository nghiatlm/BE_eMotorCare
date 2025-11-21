using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.CampaignDetailRepository
{
    public interface ICampaignDetailRepository : IGenericRepository<CampaignDetail>
    {
        Task<CampaignDetail?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<CampaignDetail> Items, long Total)> GetPagedAsync(Guid? campaignId, Guid? partId, CampaignActionType? actionType, bool? isMandatory, int? estimatedTime, int page, int pageSize);
    }
}
