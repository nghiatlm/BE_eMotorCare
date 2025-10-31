using System.Runtime.CompilerServices;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.VehicleRepository
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<Vehicle?> GetByIdAsync(Guid id) =>
            _context
                .Vehicles.AsNoTracking()
                .Include(x => x.Model) // Includes the Model entity
                .ThenInclude(m => m.MaintenancePlan) // Includes related MaintenancePlan
                .Include(x => x.Model.ModelPartTypes) // Includes related ModelPartTypes
                .Include(x => x.Customer) // Includes the Customer entity
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(IReadOnlyList<Vehicle> Items, long Total)> GetPagedAsync(
            string? search,
            StatusEnum? status,
            Guid? modelId,
            Guid? customerId,
            DateTime? fromPurchaseDate,
            DateTime? toPurchaseDate,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context
                .Vehicles.AsNoTracking()
                .Include(x => x.Model)
                .Include(x => x.Customer)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    x.VinNUmber.ToLower().Contains(s)
                    || x.ChassisNumber.ToLower().Contains(s)
                    || x.EngineNumber.ToLower().Contains(s)
                    || x.Color.ToLower().Contains(s)
                );
            }

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);
            if (modelId.HasValue)
                q = q.Where(x => x.ModelId == modelId.Value);
            if (customerId.HasValue)
                q = q.Where(x => x.CustomerId == customerId.Value);
            if (fromPurchaseDate.HasValue)
                q = q.Where(x => x.PurchaseDate.Date >= fromPurchaseDate.Value.Date);
            if (toPurchaseDate.HasValue)
                q = q.Where(x => x.PurchaseDate.Date <= toPurchaseDate.Value.Date);

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.PurchaseDate)
                .ThenBy(x => x.VinNUmber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Model?> GetModelByIdAsync(Guid modelId)
        {
            return await _context
                .Models.AsNoTracking()
                .Include(m => m.MaintenancePlan)
                .Include(m => m.Vehicles)
                .Include(m => m.ModelPartTypes)
                .FirstOrDefaultAsync(m => m.Id == modelId);
        }
    }
}
