using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.PriceServiceRepository
{
    public interface IPriceServiceRepository : IGenericRepository<PriceService>
    {
        Task<(IReadOnlyList<PriceService> Items, long Total)> GetPagedAsync(
            string? search,
            Guid? partTypeId,
            Remedies? remedies,
            DateTime? fromEffectiveDate,
            DateTime? toEffectiveDate,
            decimal? minPrice,
            decimal? maxPrice,
            int page,
            int pageSize
        );

        Task<PriceService?> GetByIdAsync(Guid id);
        Task<bool> ExistsCodeAsync(string code, Guid? ignoreId = null);
    }
}
