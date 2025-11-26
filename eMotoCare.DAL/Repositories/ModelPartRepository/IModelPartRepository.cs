using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ModelPartTypeRepository
{
    public interface IModelPartRepository : IGenericRepository<ModelPart>
    {
        Task<ModelPart?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Guid modelId, Guid partId, Guid? ignoreId = null);
        Task<(IReadOnlyList<ModelPart> Items, long Total)> GetPagedAsync(
            string? search,
            Guid? modelId,
            Guid? partId,
            Guid? id,
            int page,
            int pageSize
        );
    }
}
