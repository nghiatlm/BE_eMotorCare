using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.MaintenanceStageRepository
{
    public interface IMaintenanceStageRepository : IGenericRepository<MaintenanceStage>
    {
        Task<MaintenanceStage?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<MaintenanceStage> Items, long Total)> GetPagedAsync(Guid? maintenancePlanId, string? description, DurationMonth? durationMonth, Mileage? mileage, string? name, Status? status, int page, int pageSize);
    }
}
