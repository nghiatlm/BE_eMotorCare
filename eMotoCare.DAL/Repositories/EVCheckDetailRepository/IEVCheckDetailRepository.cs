using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.EVCheckDetailRepository
{
    public interface IEVCheckDetailRepository : IGenericRepository<EVCheckDetail>
    {
        Task<EVCheckDetail?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<EVCheckDetail> Items, long Total)> GetPagedAsync(Guid? maintenanceStageDetailId, Guid? campaignDetailId, Guid? partItemId, Guid? eVCheckId, Guid? replacePartId, string? result, Remedies? remedies, string? unit, decimal? quantity, decimal? pricePart, decimal? priceService, decimal? totalAmount, EVCheckDetailStatus? status, int page = 1, int pageSize = 10);
    }
}
