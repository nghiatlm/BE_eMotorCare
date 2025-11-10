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
            Guid? serviceCenterId,
            Guid? staffId,
            int page,
            int pageSize
        );
        Task<List<Staff>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds);
        Task<bool> ExistsCodeAsync(string code);
        Task<bool> ExistsCitizenAsync(string citizenId);
        Task<Staff?> GetByIdAsync(Guid id);
        Task DeleteAsync(Staff entity);
        Task UpdateAsync(Staff entity);
        Task CreateAsync(Staff entity);
        Task<Staff?> GetByAccountIdAsync(Guid accountId);
    }
}
