using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.CustomerRepository
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<(
            List<Appointment> appointments,
            List<EVCheck> evChecks,
            List<EVCheckDetail> evCheckDetails,
            List<Vehicle> vehicles,
            List<Model> models,
            List<MaintenanceStage> stages,
            List<VehiclePartItem> vehiclePartItems
        )> GetCustomerDetailGraphsAsync(IEnumerable<Guid> customerIds)
        {
            var ids = customerIds.Distinct().ToArray();

            var appointments = await _context
                .Appointments.AsNoTracking()
                .AsSplitQuery()
                .Where(a => ids.Contains(a.CustomerId))
                .Include(a => a.ServiceCenter)
                .Include(a => a.VehicleStage)!
                .ThenInclude(vs => vs.Vehicle)!
                .ThenInclude(v => v!.Model)
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    AppointmentDate = a.AppointmentDate,
                    ServiceCenter =
                        a.ServiceCenter == null
                            ? null
                            : new ServiceCenter
                            {
                                Id = a.ServiceCenter.Id,
                                Name = a.ServiceCenter.Name,
                            },
                    VehicleStage =
                        a.VehicleStage == null
                            ? null
                            : new VehicleStage
                            {
                                Id = a.VehicleStage.Id,
                                VehicleId = a.VehicleStage.VehicleId,
                                Vehicle =
                                    a.VehicleStage.Vehicle == null
                                        ? null
                                        : new Vehicle
                                        {
                                            Id = a.VehicleStage.Vehicle.Id,
                                            CustomerId = a.VehicleStage.Vehicle.CustomerId,
                                            ModelId = a.VehicleStage.Vehicle.ModelId,
                                            VinNUmber = a.VehicleStage.Vehicle.VinNUmber,
                                            Color = a.VehicleStage.Vehicle.Color,
                                            Status = a.VehicleStage.Vehicle.Status,
                                            Model =
                                                a.VehicleStage.Vehicle.Model == null
                                                    ? null
                                                    : new Model
                                                    {
                                                        Id = a.VehicleStage.Vehicle.Model.Id,
                                                        Name = a.VehicleStage.Vehicle.Model.Name,
                                                        MaintenancePlanId = a.VehicleStage
                                                            .Vehicle
                                                            .Model
                                                            .MaintenancePlanId,
                                                    },
                                        },
                            },
                })
                .ToListAsync();

            var appointmentIds = appointments.Select(a => a.Id).ToArray();

            var evChecks = await _context
                .EVChecks.AsNoTracking()
                .Where(e => appointmentIds.Contains(e.AppointmentId))
                .Select(e => new EVCheck
                {
                    Id = e.Id,
                    AppointmentId = e.AppointmentId,
                    CheckDate = e.CheckDate,
                    Odometer = e.Odometer,
                })
                .ToListAsync();

            var evCheckIds = evChecks.Select(e => e.Id).ToArray();

            var evCheckDetails = await _context
                .EVCheckDetails.AsNoTracking()
                .AsSplitQuery()
                .Where(d => evCheckIds.Contains(d.EVCheckId))
                .Include(d => d.MaintenanceStageDetail)!
                .ThenInclude(msd => msd.Part)
                .Include(d => d.PartItem)!
                .ThenInclude(pi => pi.Part)
                .Select(d => new EVCheckDetail
                {
                    Id = d.Id,
                    EVCheckId = d.EVCheckId,
                    Quantity = d.Quantity,
                    TotalAmount = d.TotalAmount,
                    MaintenanceStageDetail =
                        d.MaintenanceStageDetail == null
                            ? null
                            : new MaintenanceStageDetail
                            {
                                Id = d.MaintenanceStageDetail.Id,
                                Part =
                                    d.MaintenanceStageDetail.Part == null
                                        ? null
                                        : new Part
                                        {
                                            Id = d.MaintenanceStageDetail.Part.Id,
                                            Name = d.MaintenanceStageDetail.Part.Name,
                                        },
                            },
                    PartItem =
                        d.PartItem == null
                            ? null
                            : new PartItem
                            {
                                Id = d.PartItem.Id,
                                SerialNumber = d.PartItem.SerialNumber,
                                Part =
                                    d.PartItem.Part == null
                                        ? null
                                        : new Part
                                        {
                                            Id = d.PartItem.Part.Id,
                                            Code = d.PartItem.Part.Code,
                                            Name = d.PartItem.Part.Name,
                                        },
                            },
                })
                .ToListAsync();

            // ID xe xuất hiện qua appointment
            var vehicleIdsFromApps = appointments
                .Where(a => a.VehicleStage?.Vehicle != null)
                .Select(a => a.VehicleStage!.VehicleId)
                .Distinct();

            //Gộp một truy vấn Vehicles: theo CustomerId hoặc theo vehicleIdsFromApps
            var vehicles = await _context
                .Vehicles.AsNoTracking()
                .Where(v =>
                    (v.CustomerId != null && ids.Contains(v.CustomerId.Value))
                    || vehicleIdsFromApps.Contains(v.Id)
                )
                .Include(v => v.Model)
                .Select(v => new Vehicle
                {
                    Id = v.Id,
                    CustomerId = v.CustomerId,
                    ModelId = v.ModelId,
                    VinNUmber = v.VinNUmber,
                    Color = v.Color,
                    Status = v.Status,
                    Model =
                        v.Model == null
                            ? null
                            : new Model
                            {
                                Id = v.Model.Id,
                                Name = v.Model.Name,
                                MaintenancePlanId = v.Model.MaintenancePlanId,
                            },
                })
                .ToListAsync();

            var modelIds = vehicles.Select(v => v.ModelId).Distinct().ToArray();

            var models = await _context
                .Models.AsNoTracking()
                .Where(m => modelIds.Contains(m.Id))
                .Select(m => new Model
                {
                    Id = m.Id,
                    Name = m.Name,
                    MaintenancePlanId = m.MaintenancePlanId,
                })
                .ToListAsync();

            var planIds = models.Select(m => m.MaintenancePlanId).Distinct().ToArray();

            var stages = await _context
                .MaintenanceStages.AsNoTracking()
                .Where(s => planIds.Contains(s.MaintenancePlanId))
                .Select(s => new MaintenanceStage
                {
                    Id = s.Id,
                    MaintenancePlanId = s.MaintenancePlanId,
                    Name = s.Name,
                    Mileage = s.Mileage,
                    DurationMonth = s.DurationMonth,
                    EstimatedTime = s.EstimatedTime,
                    CreatedAt = s.CreatedAt,
                })
                .ToListAsync();

            var allVehicleIds = vehicles.Select(v => v.Id).Distinct().ToArray();

            var vehiclePartItems = await _context
                .VehiclePartItems.AsNoTracking()
                .Where(vpi => allVehicleIds.Contains(vpi.VehicleId))
                .Include(vpi => vpi.PartItem)!
                .ThenInclude(pi => pi.Part)
                .Select(vpi => new VehiclePartItem
                {
                    Id = vpi.Id,
                    VehicleId = vpi.VehicleId,
                    InstallDate = vpi.InstallDate,
                    PartItem =
                        vpi.PartItem == null
                            ? null
                            : new PartItem
                            {
                                Id = vpi.PartItem.Id,
                                SerialNumber = vpi.PartItem.SerialNumber,
                                Part =
                                    vpi.PartItem.Part == null
                                        ? null
                                        : new Part
                                        {
                                            Id = vpi.PartItem.Part.Id,
                                            Code = vpi.PartItem.Part.Code,
                                            Name = vpi.PartItem.Part.Name,
                                        },
                            },
                })
                .ToListAsync();

            return (
                appointments,
                evChecks,
                evCheckDetails,
                vehicles,
                models,
                stages,
                vehiclePartItems
            );
        }

        public Task<Customer?> GetByIdAsync(Guid id) =>
            _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public Task<Customer?> GetByAccountIdAsync(Guid accountId) =>
            _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.AccountId == accountId);

        public Task<bool> ExistsCitizenAsync(string citizenId, Guid? excludeCustomerId = null)
        {
            var q = _context.Customers.AsQueryable().Where(x => x.CitizenId == citizenId);
            if (excludeCustomerId.HasValue)
                q = q.Where(x => x.Id != excludeCustomerId.Value);
            return q.AnyAsync();
        }

        public Task<bool> ExistsForAccountAsync(Guid accountId) =>
            _context.Customers.AnyAsync(x => x.AccountId == accountId);

        public async Task<(IReadOnlyList<Customer> Items, long Total)> GetPagedAsync(
            string? search,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Customers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                q = q.Where(x =>
                    (x.FirstName != null && x.FirstName.ToLower().Contains(s))
                    || (x.LastName != null && x.LastName.ToLower().Contains(s))
                    || x.CitizenId.ToLower().Contains(s)
                );
            }

            var total = await q.LongCountAsync();
            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<List<Customer>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds) =>
            _context
                .Customers.AsNoTracking()
                .Where(c => accountIds.Contains(c.AccountId))
                .ToListAsync();
    }
}
