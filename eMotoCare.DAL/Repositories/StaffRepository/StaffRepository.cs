using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.StaffRepository
{
    public class StaffRepository : GenericRepository<ServiceCenter>, IStaffRepository
    {
        public StaffRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<(IReadOnlyList<Staff> Items, long Total)> GetPagedAsync(
            string? search,
            PositionEnum? position,
            Guid? serviceCenterId,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Staffs.AsNoTracking().Include(s => s.Account).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                q = q.Where(x =>
                    x.StaffCode.ToLower().Contains(search)
                    || (x.FirstName != null && x.FirstName.ToLower().Contains(search))
                    || (x.LastName != null && x.LastName.ToLower().Contains(search))
                    || (x.CitizenId != null && x.CitizenId.ToLower().Contains(search))
                );
            }

            if (position.HasValue)
                q = q.Where(x => x.Position == position.Value);

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.Staffs.AnyAsync(x => x.StaffCode == code);

        public Task<bool> ExistsCitizenAsync(string citizenId) =>
            _context.Staffs.AnyAsync(x => x.CitizenId == citizenId);

        public Task<Staff?> GetByIdAsync(Guid id) =>
            _context
                .Staffs.Include(s => s.Account)
                .AsNoTracking()
                .Include(s => s.ServiceCenter)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateAsync(Staff entity)
        {
            _context.Staffs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Staff entity)
        {
            _context.Staffs.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task<List<Staff>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds) =>
            _context
                .Staffs.AsNoTracking()
                .Include(s => s.ServiceCenter)
                .Where(s => accountIds.Contains(s.AccountId))
                .ToListAsync();

        public async Task CreateAsync(Staff entity)
        {
            _context.Staffs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task<Staff?> GetByAccountIdAsync(Guid accountId) =>
            _context.Staffs.FirstOrDefaultAsync(x => x.AccountId == accountId);
    }
}
