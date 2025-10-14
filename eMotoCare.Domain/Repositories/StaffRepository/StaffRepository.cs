using eMotoCare.Common.Enums;
using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Context;
using eMotoCare.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.StaffRepository
{
    public class StaffRepository : GenericRepository<Staff>, IStaffRepository
    {
        public StaffRepository(DBContextMotoCare context)
            : base(context) { }

        public async Task<(IReadOnlyList<Staff> Items, long Total)> GetPagedAsync(
            string? search,
            Gender? gender,
            StaffPosition? position,
            Guid? branchId,
            int page,
            int pageSize,
            CancellationToken ct = default
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Staffs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(s =>
                    s.FirstName.Contains(search)
                    || s.LastName.Contains(search)
                    || s.Address.Contains(search)
                    || s.CitizenId.Contains(search)
                    || s.StaffCode.Contains(search)
                );
            }

            if (gender.HasValue)
                q = q.Where(s => s.Gender == gender.Value);
            if (position.HasValue)
                q = q.Where(s => s.StaffPosition == position.Value);
            if (branchId.HasValue)
                q = q.Where(s => s.BranchId == branchId.Value);

            var total = await q.LongCountAsync(ct);
            var items = await q.OrderByDescending(s => s.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public Task<bool> ExistsCitizenIdAsync(
            string citizenId,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.Staffs.AnyAsync(
                s => s.CitizenId == citizenId && (exceptId == null || s.StaffId != exceptId),
                ct
            );

        public Task<bool> ExistsStaffCodeAsync(
            string staffCode,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.Staffs.AnyAsync(
                s => s.StaffCode == staffCode && (exceptId == null || s.StaffId != exceptId),
                ct
            );
    }
}
