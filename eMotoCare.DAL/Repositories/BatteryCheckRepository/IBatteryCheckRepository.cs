using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.BatteryCheckRepository
{
    public interface IBatteryCheckRepository : IGenericRepository<BatteryCheck>
    {
        Task<BatteryCheck?> GetByEVCheckDetailIdAsync(Guid evCheckDetailId);
        Task<BatteryCheck?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<BatteryCheck> Items, long Total)> GetPagedAsync(
            Guid? evCheckDetailId,
            DateTime? fromDate,
            DateTime? toDate,
            string? sortBy,
            bool sortDesc,
            int page,
            int pageSize
        );
    }
}
