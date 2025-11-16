using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.CampaignRepository
{
    public interface ICampaignRepository : IGenericRepository<Campaign>
    {
        Task<bool> ExistsCodeAsync(string code);
        Task<Campaign?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<Campaign> Items, long Total)> GetPagedAsync(
            string? code,
            string? name,
            CampaignType? campaignType,
            DateTime? fromDate,
            DateTime? toDate,
            CampaignStatus? status,
            int page,
            int pageSize
        );
        Task<List<Campaign>> GetExpiredActiveAsync(DateTime nowLocal);
    }
}
