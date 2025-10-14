using eMotoCare.Common.Enums;
using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Context;
using eMotoCare.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.BranchRepository
{
    public class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        public BranchRepository(DBContextMotoCare context)
            : base(context) { }

        public async Task<(IReadOnlyList<Branch> Items, long Total)> GetPagedAsync(
            string? search,
            Status? status,
            int page,
            int pageSize,
            CancellationToken ct = default
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Branches.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(b =>
                    b.BranchName.Contains(search)
                    || b.Address.Contains(search)
                    || b.PhoneNumber.Contains(search)
                    || (b.Email != null && b.Email.Contains(search))
                );
            }

            if (status.HasValue)
                q = q.Where(b => b.Status == status.Value);

            var total = await q.LongCountAsync(ct);

            var items = await q.OrderByDescending(b => b.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public Task<bool> ExistsNameAsync(
            string name,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.Branches.AnyAsync(
                b => b.BranchName == name && (exceptId == null || b.BranchId != exceptId),
                ct
            );

        public Task<bool> ExistsPhoneAsync(
            string phone,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.Branches.AnyAsync(
                b => b.PhoneNumber == phone && (exceptId == null || b.BranchId != exceptId),
                ct
            );

        public Task<bool> ExistsEmailAsync(
            string email,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.Branches.AnyAsync(
                b => b.Email == email && (exceptId == null || b.BranchId != exceptId),
                ct
            );

        public Task<bool> ExistsAddressAsync(
            string address,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.Branches.AnyAsync(
                b => b.Address == address && (exceptId == null || b.BranchId != exceptId),
                ct
            );
    }
}
