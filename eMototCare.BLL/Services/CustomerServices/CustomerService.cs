using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMototCare.BLL.Services.FirebaseServices;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;
        private readonly IFirebaseService _firebase;

        public CustomerService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CustomerService> logger,
            IFirebaseService firebase
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _firebase = firebase;
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
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

        public async Task<CustomerResponse?> GetAccountIdAsync(Guid id, Guid? accountId = null)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new AppException("Id khách hàng không hợp lệ", HttpStatusCode.BadRequest);

                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy khách hàng", HttpStatusCode.NotFound);
                if (accountId.HasValue && entity.AccountId != accountId.Value)
                    throw new AppException(
                        "Tài khoản không khớp với khách hàng",
                        HttpStatusCode.Forbidden
                    );

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
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.AccountId == Guid.Empty)
                    throw new AppException("AccountId không hợp lệ", HttpStatusCode.BadRequest);
                var citizen = req.CitizenId.Trim();

                if (await _unitOfWork.Customers.ExistsForAccountAsync(req.AccountId))
                    throw new AppException(
                        "Tài khoản đã có hồ sơ khách hàng",
                        HttpStatusCode.Conflict
                    );
                if (string.IsNullOrWhiteSpace(citizen))
                    throw new AppException(
                        "CitizenId không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (citizen.Length > 15)
                    throw new AppException(
                        "CitizenId không được dài hơn 15 ký tự",
                        HttpStatusCode.BadRequest
                    );
                if (await _unitOfWork.Customers.ExistsCitizenAsync(citizen))
                    throw new AppException("CCCD đã tồn tại", HttpStatusCode.Conflict);
                if (req.DateOfBirth.HasValue && req.DateOfBirth.Value.Date > DateTime.UtcNow.Date)
                    throw new AppException(
                        "Ngày sinh không được lớn hơn ngày hiện tại",
                        HttpStatusCode.BadRequest
                    );

                if (req.Gender.HasValue && !Enum.IsDefined(typeof(GenderEnum), req.Gender.Value))
                    throw new AppException("Giới tính không hợp lệ", HttpStatusCode.BadRequest);
                if (await _unitOfWork.Customers.ExistsForAccountAsync(req.AccountId))
                    throw new AppException(
                        "Tài khoản đã có hồ sơ khách hàng",
                        HttpStatusCode.Conflict
                    );

                var entity = _mapper.Map<Customer>(req);
                entity.Id = Guid.NewGuid();
                entity.CitizenId = citizen;
                var count = _unitOfWork.Customers.FindAll().Count;
                entity.CustomerCode = $"CUST-{count + 1:D5}";
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                var entity =
                    await _unitOfWork.Customers.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy khách hàng", HttpStatusCode.NotFound);

                var newCitizen = req.CitizenId.Trim();
                if (
                    !string.Equals(entity.CitizenId, newCitizen, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Customers.ExistsCitizenAsync(newCitizen, entity.Id)
                )
                    throw new AppException("CCCD đã tồn tại", HttpStatusCode.Conflict);
                if (string.IsNullOrWhiteSpace(newCitizen))
                    throw new AppException(
                        "CitizenId không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (newCitizen.Length > 15)
                    throw new AppException(
                        "CitizenId không được dài hơn 15 ký tự",
                        HttpStatusCode.BadRequest
                    );

                if (req.DateOfBirth.HasValue && req.DateOfBirth.Value.Date > DateTime.UtcNow.Date)
                    throw new AppException(
                        "Ngày sinh không được lớn hơn ngày hiện tại",
                        HttpStatusCode.BadRequest
                    );

                if (req.Gender.HasValue && !Enum.IsDefined(typeof(GenderEnum), req.Gender.Value))
                    throw new AppException("Giới tính không hợp lệ", HttpStatusCode.BadRequest);
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
        public async Task<CustomerResponse?> GetAccountIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new AppException("AccountId không hợp lệ", HttpStatusCode.BadRequest);
                var cus = await _unitOfWork.Customers.GetAccountIdAsync(id);
                if (cus == null)
                    throw new AppException("Người dùng không tồn tại", HttpStatusCode.Forbidden);

                return _mapper.Map<CustomerResponse>(cus);
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

        public async Task<bool> MapAccountIdByCitizenIdAsync(string citizenId, Guid accountId)
        {
            if (string.IsNullOrWhiteSpace(citizenId))
                throw new AppException("Citizen ID không hợp lệ", HttpStatusCode.BadRequest);
            if (accountId == Guid.Empty)
                throw new AppException("AccountId không hợp lệ", HttpStatusCode.BadRequest);
            var customer = await _unitOfWork.Customers.GetByCitizenId(citizenId);

            if (customer == null)

                throw new AppException(
                    "Không tìm thấy khách hàng có Citizen ID này",
                    HttpStatusCode.NotFound
                );

            customer.AccountId = accountId;

            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<CustomerResponse?> GetCustomerByRmaIdAsync(Guid rmaId)
        {
            if (rmaId == Guid.Empty)
                throw new AppException("RmaId không hợp lệ", HttpStatusCode.BadRequest);
            var customer = await _unitOfWork.Customers.GetByRmaId(rmaId);
            if (customer == null)
                throw new AppException(
                    "Không tìm thấy khách hàng có RMA ID này",
                    HttpStatusCode.NotFound
                );
            return _mapper.Map<CustomerResponse>(customer);
        }

        public async Task<SyncCustomerResponse> SyncCustomerAsync(Guid accountId, string citizenId, string chassisNumber)
        {
            try
            {
                if (accountId == Guid.Empty)
                    throw new AppException("AccountId không hợp lệ", HttpStatusCode.BadRequest);

                if (string.IsNullOrWhiteSpace(citizenId))
                    throw new AppException("CitizenId không hợp lệ", HttpStatusCode.BadRequest);

                var customerInfo = await _unitOfWork.Customers.GetByAccountIdAsync(accountId);
                 if (customerInfo != null)
                {
                    throw new AppException(
                        "Tài khoản đã có hồ sơ khách hàng",
                        HttpStatusCode.Conflict
                    );
                }

                 var vehicleExist = await _firebase.GetVehicleByChassisNumberAsync(chassisNumber);
                if (vehicleExist == null)
                {
                    throw new AppException(
                        "Không tìm thấy xe trong hệ thống",
                        HttpStatusCode.NotFound
                    );
                }

                    if (!_firebase.IsFirestoreConfigured())
                {
                    _logger.LogWarning(
                        "Firestore not configured - cannot sync customer data for citizenId={CitizenId}",
                        citizenId
                    );
                    return null;
                }

                var customerExist = await _unitOfWork.Customers.GetByCitizenId(citizenId);
                if (customerExist != null)
                {
                    customerExist.AccountId = accountId;

                    _unitOfWork.Customers.Update(customerExist);
                    var syncPlan = await _firebase.GetMaintenancePlanAsync();
                    if (!syncPlan) throw new AppException("Sync plan thất bại");
                    var syncStage = await _firebase.GetMaintenanceStageAsync();
                    if (!syncStage) throw new AppException("syncStage thất bại");
                    var syncStageDetail = await _firebase.GetMaintenanceStageDetailAsync();
                    if (!syncStageDetail) throw new AppException("syncStageDetail thất bại");
                    var syncModel = await _firebase.GetModelAsync();
                    if (!syncModel) throw new AppException("Sync model thất bại");
                    var syncVehicle = await _firebase.CreateVehicleByChassis(chassisNumber);
                    if (!syncVehicle) throw new AppException("Sync vehicle thất bại");
                    //var syncPartItem = await _firebase.GetPartItemAsync();
                    //if (!syncPartItem) throw new AppException("Sync part item thất bại");
                    var vehicle = await _unitOfWork.Vehicles.GetByChassisNumberAsync(chassisNumber);
                    var syncVehiclePartItem = await _firebase.CreateVehiclePartItemsByVehicleIdAsync(vehicle.Id.ToString());
                    if (!syncVehiclePartItem) throw new AppException("Sync vehicle part item thất bại");
                    var syncVehicleStage = await _firebase.CreateVehicleStageByVehicleId(vehicle.Id.ToString());
                    if (!syncVehicleStage) throw new AppException("Sync vehicle stage thất bại");

                    var vehicleResponse = _mapper.Map<VehicleResponse>(vehicle);
                    var response = new SyncCustomerResponse
                    {
                        FirstName = customerExist.FirstName,
                        LastName = customerExist.LastName,
                        CitizenId = customerExist.CitizenId,
                        DateOfBirth = customerExist.DateOfBirth,
                        Address = customerExist.Address,
                        Gender = customerExist.Gender,
                        VehicleResponse = vehicleResponse
                    };
                    await _unitOfWork.SaveAsync();
                    return response;
                } else
                {
                    var resultCustomer = await _firebase.GetCustomerByCitizenIdAsync(citizenId, accountId);
                    if (resultCustomer == false)
                        throw new AppException(
                            "Không tìm thấy khách hàng trong hệ thống",
                            HttpStatusCode.NotFound
                        );
                    var customer = await _unitOfWork.Customers.GetByCitizenId(citizenId);
                    var syncPlan = await _firebase.GetMaintenancePlanAsync();
                    if (!syncPlan) throw new AppException("Sync plan thất bại");
                    var syncStage = await _firebase.GetMaintenanceStageAsync();
                    if (!syncStage) throw new AppException("syncStage thất bại");
                    var syncStageDetail = await _firebase.GetMaintenanceStageDetailAsync();
                    if (!syncStageDetail) throw new AppException("syncStageDetail thất bại");
                    var syncModel = await _firebase.GetModelAsync();
                    if (!syncModel) throw new AppException("Sync model thất bại");
                    var syncVehicle = await _firebase.CreateVehicleByChassis(chassisNumber);
                    if (!syncVehicle) throw new AppException("Sync vehicle thất bại");
                    //var syncPartItem = await _firebase.GetPartItemAsync();
                    //if (!syncPartItem) throw new AppException("Sync part item thất bại");
                    var vehicle = await _unitOfWork.Vehicles.GetByChassisNumberAsync(chassisNumber);
                    var syncVehiclePartItem = await _firebase.CreateVehiclePartItemsByVehicleIdAsync(vehicle.Id.ToString());
                    if (!syncVehiclePartItem) throw new AppException("Sync vehicle part item thất bại");
                    var syncVehicleStage = await _firebase.CreateVehicleStageByVehicleId(vehicle.Id.ToString());
                    if (!syncVehicleStage) throw new AppException("Sync vehicle stage thất bại");

                    var vehicleResponse = _mapper.Map<VehicleResponse>(vehicle);
                    var response = new SyncCustomerResponse
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        CitizenId = customer.CitizenId,
                        DateOfBirth = customer.DateOfBirth,
                        Address = customer.Address,
                        Gender = customer.Gender,
                        VehicleResponse = vehicleResponse,
                    };
                    await _unitOfWork.SaveAsync();
                    return response;
                }
     
                
                
                
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
