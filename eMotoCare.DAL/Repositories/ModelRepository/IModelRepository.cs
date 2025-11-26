using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ModelRepository
{
    public interface IModelRepository : IGenericRepository<Model>
    {
        Task<Model?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<Model> Items, long Total)> GetPagedAsync(
            string? search,
            Status? status,
            Guid? modelId,
            Guid? maintenancePlanId,
            int page,
            int pageSize
        );

        Task<bool> ExistsCodeAsync(string code);
        Task<bool> ExistsNameAsync(string name, Guid? ignoreId = null);
    }
}
