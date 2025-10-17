using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.MaintenanceStageDetailRepository
{
    public interface IMaintenanceStageDetailRepository : IGenericRepository<MaintenanceStageDetail>
    {
        Task<MaintenanceStageDetail?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<MaintenanceStageDetail> Items, long Total)> GetPagedAsync(Guid? maintenanceStageId, Guid? partId, ActionType? actionType, string? description, int page, int pageSize);
    }
}
