using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.RMADetailRepository
{
    public interface IRMADetailRepository : IGenericRepository<RMADetail>
    {
        Task<bool> ExistsRmaNumberAsync(string rmaNumber);
        Task<RMADetail?> GetByEvCheckDetail(Guid id);
        Task<RMADetail?> GetByIdAsync(Guid id);
        Task<List<RMADetail?>> GetByRmaId(Guid id);
        Task<(IReadOnlyList<RMADetail> Items, long Total)> GetPagedAsync(string? rmaNumber, string? inspector, string? result, string? solution, Guid? evCheckDetailId, Guid? rmaId, RMADetailStatus? status, int page, int pageSize);
    }
}
