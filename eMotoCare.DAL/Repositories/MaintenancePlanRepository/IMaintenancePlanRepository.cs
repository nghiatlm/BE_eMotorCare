using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.MaintenancePlanRepository
{
    public interface IMaintenancePlanRepository : IGenericRepository<MaintenancePlan> {
        Task<bool> ExistsCodeAsync(string code);
        Task<bool> ExistsNameAsync(string name);
        Task<MaintenancePlan?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<MaintenancePlan> Items, long Total)> GetPagedAsync(string? code, string? description, string? name, int? totalStage, Status? status, MaintenanceUnit[]? maintenanceUnit, int page, int pageSize);
    }
}
