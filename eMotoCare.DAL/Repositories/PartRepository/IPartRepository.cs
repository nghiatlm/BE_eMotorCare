using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.PartRepository
{
    public interface IPartRepository : IGenericRepository<Part>
    {
        Task<bool> ExistsCodeAsync(string code);
        Task<bool> ExistsNameAsync(string name);
        Task<Part?> GetByIdAsync(Guid id);
        Task<List<Part>> FindPartTypeAsync(Guid partTypeId);
        Task<(IReadOnlyList<Part> Items, long Total)> GetPagedAsync(Guid? partTypeId, string? code, string? name, Status? status, int? quantity, int page, int pageSize);
    }
}
