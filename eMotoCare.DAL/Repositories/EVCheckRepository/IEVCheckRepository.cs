using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.EVCheckRepository
{
    public interface IEVCheckRepository : IGenericRepository<EVCheck>
    {
        Task<EVCheck?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<EVCheck> Items, long Total)> GetPagedAsync(DateTime? startDate, DateTime? endDate, EVCheckStatus? status, Guid? appointmentId, Guid? taskExecutorId, int page, int pageSize);
    }
}
