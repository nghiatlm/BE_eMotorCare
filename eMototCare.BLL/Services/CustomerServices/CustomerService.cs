using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CustomerService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<object> GetPagedViewAsync(string? search, int page, int pageSize)
        {
            try
            {
                var (customerEntities, total) = await _unitOfWork.Customers.GetPagedAsync(
                    search,
                    page,
                    pageSize
                );
                var customers = _mapper.Map<List<CustomerResponse>>(customerEntities);
                var customerIds = customers.Select(c => c.Id).ToArray();

                var g = await _unitOfWork.Customers.GetCustomerDetailGraphsAsync(customerIds);
                var appointments = g.appointments ?? g.Item1;
                var evChecks = g.evChecks ?? g.Item2;
                var evCheckDetails = g.evCheckDetails ?? g.Item3;
                var vehicles = g.vehicles ?? g.Item4;
                var models = g.models ?? g.Item5;
                var stages = g.stages ?? g.Item6;
                var vehiclePartItems = g.vehiclePartItems ?? g.Item7;

                var stagesByPlan = stages
                    .GroupBy(s => s.MaintenancePlanId)
                    .ToDictionary(gp => gp.Key, gp => gp.OrderBy(x => x.CreatedAt).ToList());
                var evByAppt = evChecks.ToDictionary(e => e.AppointmentId, e => e);
                var detsByEv = evCheckDetails
                    .GroupBy(d => d.EVCheckId)
                    .ToDictionary(gp => gp.Key, gp => gp.ToList());
                var appsByCust = appointments
                    .GroupBy(a => a.CustomerId)
                    .ToDictionary(
                        gp => gp.Key,
                        gp => gp.OrderByDescending(x => x.AppointmentDate).ToList()
                    );
                var vehIdByAppt = appointments
                    .Where(a => a.VehicleStage?.Vehicle != null)
                    .ToDictionary(a => a.Id, a => a.VehicleStage!.VehicleId);
                var vpiByVeh = vehiclePartItems
                    .GroupBy(v => v.VehicleId)
                    .ToDictionary(
                        gp => gp.Key,
                        gp => gp.OrderByDescending(x => x.InstallDate).ToList()
                    );
                var vehByCust = vehicles
                    .Where(v => v.CustomerId != null)
                    .GroupBy(v => v.CustomerId!.Value)
                    .ToDictionary(gp => gp.Key, gp => gp.ToList());

                static void AddIf(Dictionary<string, object> d, string k, object? v)
                {
                    if (v is string s && string.IsNullOrWhiteSpace(s))
                        return;
                    if (v != null)
                        d[k] = v;
                }
                static void AddIfAny(Dictionary<string, object> d, string k, IList<object> list)
                {
                    if (list != null && list.Count > 0)
                        d[k] = list;
                }

                List<object> BuildSchedule(Vehicle v)
                {
                    if (
                        v.Model == null
                        || !stagesByPlan.TryGetValue(v.Model.MaintenancePlanId, out var st)
                    )
                        return new();
                    return st.Select(s =>
                            (object)
                                new
                                {
                                    stageName = s.Name,
                                    mileage = s.Mileage.ToString(),
                                    durationMonth = s.DurationMonth.ToString(),
                                    estimatedTime = s.EstimatedTime,
                                }
                        )
                        .ToList();
                }

                List<object> BuildHistory(Guid vehicleId, IEnumerable<Appointment> apps)
                {
                    var hist = new List<object>();
                    foreach (
                        var ap in apps.Where(ap =>
                            vehIdByAppt.TryGetValue(ap.Id, out var vid) && vid == vehicleId
                        )
                    )
                    {
                        int? odo = null;
                        decimal? total = null;
                        var works = new List<string>();
                        if (
                            evByAppt.TryGetValue(ap.Id, out var ev)
                            && detsByEv.TryGetValue(ev.Id, out var dets)
                        )
                        {
                            odo = ev.Odometer;
                            total = dets.Where(x => x.TotalAmount.HasValue)
                                .Sum(x => x.TotalAmount!.Value);
                            foreach (var d in dets)
                            {
                                var pn = d.MaintenanceStageDetail?.Part?.Name;
                                if (!string.IsNullOrWhiteSpace(pn))
                                    works.Add(pn);
                                if (d.PartItem?.Part?.Name != null)
                                    works.Add($"Thay {d.PartItem.Part.Name}");
                            }
                        }

                        var row = new Dictionary<string, object> { ["date"] = ap.AppointmentDate };
                        AddIf(row, "serviceCenter", ap.ServiceCenter?.Name);
                        AddIf(row, "odometer", odo);
                        AddIf(row, "totalAmount", total);
                        if (works.Count > 0)
                            row["works"] = works;
                        hist.Add(row);
                    }
                    return hist;
                }

                List<object> BuildReplacements(Guid vehicleId, IEnumerable<Appointment> apps)
                {
                    var reps = new List<object>();

                    if (vpiByVeh.TryGetValue(vehicleId, out var vpis))
                    {
                        reps.AddRange(
                            vpis.Select(r =>
                            {
                                var row = new Dictionary<string, object>
                                {
                                    ["date"] = r.InstallDate,
                                    ["quantity"] = 1,
                                    ["source"] = "Install",
                                };
                                AddIf(row, "partCode", r.PartItem?.Part?.Code);
                                AddIf(row, "partName", r.PartItem?.Part?.Name);
                                AddIf(row, "serialNumber", r.PartItem?.SerialNumber);
                                return (object)row;
                            })
                        );
                    }

                    foreach (
                        var ap in apps.Where(ap =>
                            vehIdByAppt.TryGetValue(ap.Id, out var vid) && vid == vehicleId
                        )
                    )
                    {
                        if (
                            evByAppt.TryGetValue(ap.Id, out var ev)
                            && detsByEv.TryGetValue(ev.Id, out var dets)
                        )
                        {
                            reps.AddRange(
                                dets.Where(d => d.PartItem != null)
                                    .Select(d =>
                                    {
                                        var row = new Dictionary<string, object>
                                        {
                                            ["date"] = ev.CheckDate,
                                            ["source"] = "EVCheck",
                                        };
                                        AddIf(row, "partCode", d.PartItem!.Part?.Code);
                                        AddIf(row, "partName", d.PartItem!.Part?.Name);
                                        AddIf(row, "serialNumber", d.PartItem!.SerialNumber);
                                        AddIf(row, "quantity", (int?)(d.Quantity ?? 1));
                                        return (object)row;
                                    })
                            );
                        }
                    }

                    return reps;
                }

                object BuildVehicle(Vehicle v, IEnumerable<Appointment> apps)
                {
                    var obj = new Dictionary<string, object> { ["id"] = v.Id };
                    AddIf(obj, "vinNumber", v.VinNUmber);
                    AddIf(obj, "modelName", v.Model?.Name);
                    AddIf(obj, "color", v.Color);
                    AddIf(obj, "status", v.Status.ToString());

                    var schedule = BuildSchedule(v);
                    var history = BuildHistory(v.Id, apps);
                    var reps = BuildReplacements(v.Id, apps);

                    AddIfAny(obj, "maintenanceSchedule", schedule);
                    AddIfAny(obj, "maintenanceHistory", history);
                    AddIfAny(obj, "partReplacements", reps);
                    return obj;
                }

                object BuildCustomer(CustomerResponse c)
                {
                    var obj = new Dictionary<string, object> { ["id"] = c.Id };
                    AddIf(obj, "accountId", c.AccountId);
                    AddIf(obj, "firstName", c.FirstName);
                    AddIf(obj, "lastName", c.LastName);
                    AddIf(obj, "address", c.Address);
                    AddIf(obj, "citizenId", c.CitizenId);
                    AddIf(obj, "dateOfBirth", c.DateOfBirth);
                    AddIf(obj, "gender", c.Gender);
                    AddIf(obj, "avatarUrl", c.AvatarUrl);
                    AddIf(obj, "createdAt", c.CreatedAt);
                    AddIf(obj, "updatedAt", c.UpdatedAt);

                    var apps = appsByCust.TryGetValue(c.Id, out var a)
                        ? a
                        : Enumerable.Empty<Appointment>();
                    var vehs = vehByCust.TryGetValue(c.Id, out var listV)
                        ? listV
                        : new List<Vehicle>();
                    var vehObjs = vehs.Select(v => BuildVehicle(v, apps)).ToList();
                    if (vehs.Count > 0)
                        obj["vehicles"] = vehObjs;

                    return obj;
                }

                var items = customers.Select(BuildCustomer).ToList();

                return new
                {
                    page,
                    pageSize,
                    total = (int)total,
                    items,
                };
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPagedViewAsync failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PageResult<CustomerResponse>> GetPagedAsync(
            string? search,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Customers.GetPagedAsync(
                    search,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<CustomerResponse>>(items);
                return new PageResult<CustomerResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy khách hàng", HttpStatusCode.NotFound);
                return _mapper.Map<CustomerResponse>(entity);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Guid> CreateAsync(CustomerRequest req)
        {
            try
            {
                var citizen = req.CitizenId.Trim();

                if (await _unitOfWork.Customers.ExistsForAccountAsync(req.AccountId))
                    throw new AppException(
                        "Tài khoản đã có hồ sơ khách hàng",
                        HttpStatusCode.Conflict
                    );

                if (await _unitOfWork.Customers.ExistsCitizenAsync(citizen))
                    throw new AppException("CCCD đã tồn tại", HttpStatusCode.Conflict);

                var entity = _mapper.Map<Customer>(req);
                entity.Id = Guid.NewGuid();
                entity.CitizenId = citizen;

                await _unitOfWork.Customers.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Created Customer {Id} (Account {AccountId})",
                    entity.Id,
                    entity.AccountId
                );
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, CustomerRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy khách hàng", HttpStatusCode.NotFound);

                var newCitizen = req.CitizenId.Trim();
                if (
                    !string.Equals(entity.CitizenId, newCitizen, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Customers.ExistsCitizenAsync(newCitizen, entity.Id)
                )
                    throw new AppException("CCCD đã tồn tại", HttpStatusCode.Conflict);

                _mapper.Map(req, entity);
                entity.CitizenId = newCitizen;

                await _unitOfWork.Customers.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Customer {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy khách hàng", HttpStatusCode.NotFound);

                await _unitOfWork.Customers.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Customer {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Customer failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
