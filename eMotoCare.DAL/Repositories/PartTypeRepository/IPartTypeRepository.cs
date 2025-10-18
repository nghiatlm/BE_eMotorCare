using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.PartTypeRepository
{
    public interface IPartTypeRepository : IGenericRepository<PartType>
    {
        Task<(IReadOnlyList<PartType> Items, long Total)> GetPagedAsync(string? name, string? description, int page, int pageSize);
    }
}
