using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eMotoCare.DAL.Bases;
using eMotoCare.DAL.Context;
using eMotoCare.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ServiceCenterRepository
{
    public class ServiceCenterRepository
        : GenericRepository<ServiceCenter>,
            IServiceCenterRepository
    {
        public ServiceCenterRepository(DBContextMotoCare context)
            : base(context) { }

        public async Task<(IReadOnlyList<ServiceCenter> Items, long Total)> GetPagedAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken ct = default
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.ServiceCenters.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                q = q.Where(x =>
                    x.CenterName.Contains(search)
                    || x.Address.Contains(search)
                    || x.PhoneNumber.Contains(search)
                    || (x.Email != null && x.Email.Contains(search))
                );
            }

            var total = await q.LongCountAsync(ct);
            var items = await q.OrderByDescending(x => x.CreatedAt)
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
            _context.ServiceCenters.AnyAsync(
                x => x.CenterName == name && (exceptId == null || x.ServiceCenterId != exceptId),
                ct
            );

        public Task<bool> ExistsPhoneAsync(
            string phone,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.ServiceCenters.AnyAsync(
                x => x.PhoneNumber == phone && (exceptId == null || x.ServiceCenterId != exceptId),
                ct
            );

        public Task<bool> ExistsEmailAsync(
            string email,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.ServiceCenters.AnyAsync(
                x => x.Email == email && (exceptId == null || x.ServiceCenterId != exceptId),
                ct
            );

        public Task<bool> ExistsAddressAsync(
            string address,
            Guid? exceptId = null,
            CancellationToken ct = default
        ) =>
            _context.ServiceCenters.AnyAsync(
                x => x.Address == address && (exceptId == null || x.ServiceCenterId != exceptId),
                ct
            );
    }
}
