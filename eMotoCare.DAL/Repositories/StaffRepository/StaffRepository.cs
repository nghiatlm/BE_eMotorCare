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
            Guid? staffId,
            Guid? accountId,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Staffs.AsNoTracking().Include(x => x.Account).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                q = q.Where(x =>
                    x.StaffCode.ToLower().Contains(search)
                    || (x.FirstName != null && x.FirstName.ToLower().Contains(search))
                    || (x.LastName != null && x.LastName.ToLower().Contains(search))
                    || (x.CitizenId != null && x.CitizenId.ToLower().Contains(search))
                    || (x.Account != null && x.Account.Phone.ToLower().Contains(search))
                );
            }
            if (serviceCenterId.HasValue)
                q = q.Where(x => x.ServiceCenterId == serviceCenterId.Value);
            if (position.HasValue)
                q = q.Where(x => x.Position == position.Value);
            if (staffId.HasValue)
                q = q.Where(x => x.Id == staffId.Value);
            if (accountId.HasValue)
                q = q.Where(x => x.AccountId == accountId.Value);

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
                .Include(x => x.Account)
                .Include(s => s.ServiceCenter)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateAsync(Staff entity)
        {
            _context.Staffs.Update(entity);
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

        public async Task<List<Staff>?> GetAvailableTechnicianAsync(int slotTime, DateTime appointmentDate)
        {
            var busyStaffIds = await _context.Appointments
                            .Include(x => x.EVCheck)
                            .Where(a => (int)a.SlotTime == slotTime && a.AppointmentDate == appointmentDate)
                            .Select(a => a.EVCheck.TaskExecutorId)
                            .Distinct()
                            .ToListAsync();

            var availableStaffs = await _context.Staffs
                            .Where(s => !busyStaffIds.Contains(s.Id)
                                     && s.Position == PositionEnum.TECHNICIAN_STAFF)
                            .ToListAsync();

            return availableStaffs;
        }
    }
}
