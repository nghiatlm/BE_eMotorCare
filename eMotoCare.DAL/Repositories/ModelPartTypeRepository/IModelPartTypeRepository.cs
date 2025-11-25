using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ModelPartTypeRepository
{
    public interface IModelPartTypeRepository : IGenericRepository<ModelPartType>
    {
        Task<ModelPartType?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid modelId, Guid partTypeId, Guid? ignoreId = null);
        Task<(IReadOnlyList<ModelPartType> Items, long Total)> GetPagedAsync(
            string? search,
            Guid? id,
            Guid? modelId,
            Guid? partTypeId,
            Status? status,
            int page,
            int pageSize
        );
    }
}
