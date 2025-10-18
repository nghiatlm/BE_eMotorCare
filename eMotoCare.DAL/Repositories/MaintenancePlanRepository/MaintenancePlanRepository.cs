using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace eMotoCare.DAL.Repositories.MaintenancePlanRepository
{
    public class MaintenancePlanRepository : GenericRepository<MaintenancePlan>, IMaintenancePlanRepository
    {
        public MaintenancePlanRepository(ApplicationDbContext context) : base(context)
        {

        }

        public Task<MaintenancePlan?> GetByIdAsync(Guid id) =>
            _context.MaintenancePlans.FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.MaintenancePlans.AnyAsync(x => x.Code == code);

        public Task<bool> ExistsNameAsync(string name) =>
            _context.MaintenancePlans.AnyAsync(x => x.Name == name);

        public async Task<(IReadOnlyList<MaintenancePlan> Items, long Total)> GetPagedAsync(
           string? code,
            string? description,
            string? name,
            int? totalStage,
            Status? status,
            MaintenanceUnit[]? maintenanceUnit,
            int page,
            int pageSize
       )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.MaintenancePlans
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(code))
            {
                q = q.Where(x =>
                    x.Code.Contains(code));
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                q = q.Where(x =>
                    x.Description.Contains(description));
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                q = q.Where(x =>
                    x.Name.Contains(name));
            }
            if (totalStage.HasValue)
                q = q.Where(x => x.TotalStages == totalStage.Value);
            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (maintenanceUnit != null && maintenanceUnit.Any())
            {
                q = q.Where(x => x.Unit.Any(u => maintenanceUnit.Contains(u)));
            }



            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
