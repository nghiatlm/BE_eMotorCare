using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.PartTypeRepository
{
    public interface IPartTypeRepository : IGenericRepository<PartType>
    {
        Task<bool> ExistsNameAsync(string name);
        Task<PartType?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<PartType> Items, long Total)> GetPagedAsync(string? name, string? description, int page, int pageSize);
    }
}
