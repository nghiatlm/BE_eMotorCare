using eMotoCare.Common.Enums;
using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Entities;

namespace eMotoCare.DAL.Repositories.StaffRepository
{
    public interface IStaffRepository : IGenericRepository<Staff>
    {
        Task<(IReadOnlyList<Staff> Items, long Total)> GetPagedAsync(
            string? search,
            Gender? gender,
            StaffPosition? position,
            Guid? branchId,
            int page,
            int pageSize,
            CancellationToken ct = default
        );

        Task<bool> ExistsCitizenIdAsync(
            string citizenId,
            Guid? exceptId = null,
            CancellationToken ct = default
        );
        Task<bool> ExistsStaffCodeAsync(
            string staffCode,
            Guid? exceptId = null,
            CancellationToken ct = default
        );
    }
}
