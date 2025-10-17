using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.StaffRepository
{
    public interface IStaffRepository : IGenericRepository<ServiceCenter>
    {
        Task<(IReadOnlyList<Staff> Items, long Total)> GetPagedAsync(
            string? search,
            PositionEnum? position,
            int page,
            int pageSize
        );

        Task<bool> ExistsCodeAsync(string code);
        Task<bool> ExistsCitizenAsync(string citizenId);
        Task<Staff?> GetByIdAsync(Guid id);
        Task DeleteAsync(Staff entity);
        Task UpdateAsync(Staff entity);
        Task CreateAsync(Staff entity);
    }
}
