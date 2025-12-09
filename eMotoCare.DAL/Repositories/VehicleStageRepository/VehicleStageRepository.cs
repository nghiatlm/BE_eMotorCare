using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.VehicleStageRepository
{
    public class VehicleStageRepository : GenericRepository<VehicleStage>, IVehicleStageRepository
    {
        public VehicleStageRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<List<VehicleStage>> GetByVehicleIdAsync(Guid vehicleId)
        {
            return _context.VehicleStages.Include(x => x.MaintenanceStage).Where(vs => vs.VehicleId == vehicleId).ToListAsync();
        }

        public Task<VehicleStage?> GetByIdAsync(Guid id) =>
            _context
                .VehicleStages.AsNoTracking()
                .Include(x => x.Vehicle)
                .ThenInclude(v => v.Model)
                .Include(x => x.MaintenanceStage)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(IReadOnlyList<VehicleStage> Items, long Total)> GetPagedAsync(
            Guid? vehicleId,
            Guid? maintenanceStageId,
            VehicleStageStatus? status,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context
                .VehicleStages.AsNoTracking()
                .Include(x => x.Vehicle)
                .ThenInclude(v => v.Model)
                .Include(x => x.MaintenanceStage)
                .AsQueryable();

            if (vehicleId.HasValue)
                q = q.Where(x => x.VehicleId == vehicleId.Value);
            if (maintenanceStageId.HasValue)
                q = q.Where(x => x.MaintenanceStageId == maintenanceStageId.Value);
            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (fromDate.HasValue)
            {
                var from = fromDate.Value.Date;
                q = q.Where(x =>
                    x.ExpectedImplementationDate.HasValue
                    && x.ExpectedImplementationDate.Value.Date >= from
                );
            }
            if (toDate.HasValue)
            {
                var to = toDate.Value.Date;
                q = q.Where(x =>
                    x.ExpectedImplementationDate.HasValue
                    && x.ExpectedImplementationDate.Value.Date <= to
                );
            }

            var total = await q.LongCountAsync();
            var items = await q.OrderBy(x =>
                    x.ExpectedImplementationDate ?? x.ActualImplementationDate ?? DateTime.MaxValue
                )
                .ThenBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
