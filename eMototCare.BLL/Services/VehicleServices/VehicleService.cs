using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMototCare.BLL.Services.FirebaseServices;
using eMototCare.BLL.Services.ModelServices;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.VehicleServices
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleService> _logger;
        private readonly IFirebaseService _firebase;
        private readonly IModelService _modelService;

        public VehicleService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<VehicleService> logger,
            IFirebaseService firebase,
            IModelService modelService
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _firebase = firebase;
            _modelService = modelService;
        }

        public async Task<PageResult<VehicleResponse>> GetPagedAsync(
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
            try
            {
                var (items, total) = await _unitOfWork.Vehicles.GetPagedAsync(
                    search,
                    status,
                    modelId,
                    customerId,
                    fromPurchaseDate,
                    toPurchaseDate,
                    page,
                    pageSize
                );
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (status.HasValue && !Enum.IsDefined(typeof(StatusEnum), status.Value))
                    throw new AppException("Trạng thái xe không hợp lệ", HttpStatusCode.BadRequest);

                if (modelId.HasValue && modelId.Value == Guid.Empty)
                    throw new AppException("ModelId không hợp lệ", HttpStatusCode.BadRequest);

                if (customerId.HasValue && customerId.Value == Guid.Empty)
                    throw new AppException("CustomerId không hợp lệ", HttpStatusCode.BadRequest);

                if (
                    fromPurchaseDate.HasValue
                    && toPurchaseDate.HasValue
                    && fromPurchaseDate > toPurchaseDate
                )
                {
                    throw new AppException(
                        "fromPurchaseDate không được lớn hơn toPurchaseDate",
                        HttpStatusCode.BadRequest
                    );
                }
                var rows = _mapper.Map<List<VehicleResponse>>(items);
                return new PageResult<VehicleResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<VehicleResponse?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
            var v =
                await _unitOfWork.Vehicles.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.NotFound);
            return _mapper.Map<VehicleResponse>(v);
        }

        public async Task<Guid> CreateAsync(VehicleRequest req)
        {
            try
            {
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                var image = (req.Image ?? string.Empty).Trim();
                var color = (req.Color ?? string.Empty).Trim();
                var chassis = (req.ChassisNumber ?? string.Empty).Trim();
                var engine = (req.EngineNumber ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(image))
                    throw new AppException("Image không được để trống", HttpStatusCode.BadRequest);

                if (string.IsNullOrWhiteSpace(color))
                    throw new AppException("Màu xe không được để trống", HttpStatusCode.BadRequest);

                if (string.IsNullOrWhiteSpace(chassis))
                    throw new AppException(
                        "Số khung (ChassisNumber) không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (string.IsNullOrWhiteSpace(engine))
                    throw new AppException(
                        "Số máy (EngineNumber) không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (!Enum.IsDefined(typeof(StatusEnum), req.Status))
                    throw new AppException("Trạng thái xe không hợp lệ", HttpStatusCode.BadRequest);

                if (req.ModelId == Guid.Empty)
                    throw new AppException("ModelId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.ManufactureDate == default)
                    throw new AppException(
                        "ManufactureDate không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (req.PurchaseDate == default)
                    throw new AppException("PurchaseDate không hợp lệ", HttpStatusCode.BadRequest);

                if (req.WarrantyExpiry == default)
                    throw new AppException(
                        "WarrantyExpiry không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (req.ManufactureDate > req.PurchaseDate)
                    throw new AppException(
                        "Ngày sản xuất không được lớn hơn ngày mua",
                        HttpStatusCode.BadRequest
                    );

                if (req.PurchaseDate > req.WarrantyExpiry)
                    throw new AppException(
                        "Ngày mua không được lớn hơn ngày hết bảo hành",
                        HttpStatusCode.BadRequest
                    );

                if (req.CustomerId.HasValue && req.CustomerId.Value == Guid.Empty)
                    throw new AppException("CustomerId không hợp lệ", HttpStatusCode.BadRequest);
                var model =
                    await _unitOfWork.Models.GetByIdAsync(req.ModelId)
                    ?? throw new AppException("Không tìm thấy Model", HttpStatusCode.BadRequest);

                if (req.CustomerId.HasValue)
                {
                    var customer = await _unitOfWork.Customers.GetByIdAsync(req.CustomerId.Value);
                    if (customer == null)
                        throw new AppException(
                            "Không tìm thấy khách hàng",
                            HttpStatusCode.BadRequest
                        );
                }
                var entity = _mapper.Map<Vehicle>(req);
                entity.Id = Guid.NewGuid();
                entity.Image = image;
                entity.Color = color;
                entity.ChassisNumber = chassis;
                entity.EngineNumber = engine;

                if (entity.CustomerId.HasValue && req.IsPrimary == true)
                {
                    var others = await _unitOfWork.Vehicles.GetByCustomerIdAsync(entity.CustomerId.Value);
                    foreach (var v in others.Where(x => x.IsPrimary))
                    {
                        v.IsPrimary = false;
                        await _unitOfWork.Vehicles.UpdateAsync(v);
                    }
                    entity.IsPrimary = true;
                }
                else
                {
                    entity.IsPrimary = false;
                }
                await _unitOfWork.Vehicles.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Created Vehicle {Id})", entity.Id);
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, VehicleUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Vehicles.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.NotFound);
                var newImage = (req.Image ?? entity.Image ?? string.Empty).Trim();      
                var newColor = (req.Color ?? entity.Color ?? string.Empty).Trim();      
                var newStatus = req.Status ?? entity.Status;                            
                var newModelId = req.ModelId ?? entity.ModelId;                         
                var newPurchaseDate = req.PurchaseDate ?? entity.PurchaseDate;         
                var newWarrantyExpiry = req.WarrantyExpiry ?? entity.WarrantyExpiry;   
                var newCustomerId = req.CustomerId ?? entity.CustomerId;              
                var newIsPrimary = req.IsPrimary ?? entity.IsPrimary;
                if (string.IsNullOrWhiteSpace(newImage))
                    throw new AppException("Image không được để trống", HttpStatusCode.BadRequest);

                if (string.IsNullOrWhiteSpace(newColor))
                    throw new AppException("Màu xe không được để trống", HttpStatusCode.BadRequest);

                if (!Enum.IsDefined(typeof(StatusEnum), newStatus))
                    throw new AppException("Trạng thái xe không hợp lệ", HttpStatusCode.BadRequest);

                if (newModelId == Guid.Empty)
                    throw new AppException("ModelId không hợp lệ", HttpStatusCode.BadRequest);

                if (newPurchaseDate == default)
                    throw new AppException("PurchaseDate không hợp lệ", HttpStatusCode.BadRequest);

                if (newWarrantyExpiry == default)
                    throw new AppException(
                        "WarrantyExpiry không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (entity.ManufactureDate > newPurchaseDate)
                    throw new AppException(
                        "Ngày sản xuất không được lớn hơn ngày mua",
                        HttpStatusCode.BadRequest
                    );

                if (newPurchaseDate > newWarrantyExpiry)
                    throw new AppException(
                        "Ngày mua không được lớn hơn ngày hết bảo hành",
                        HttpStatusCode.BadRequest
                    );

                if (newCustomerId.HasValue && newCustomerId.Value == Guid.Empty)
                    throw new AppException("CustomerId không hợp lệ", HttpStatusCode.BadRequest);
                if (entity.CustomerId.HasValue)
                {
                    var oldCustomerId = entity.CustomerId.Value;
                    var isCurrentlyPrimary = entity.IsPrimary;
                    var willBelongToOldCustomer =
                        newCustomerId.HasValue && newCustomerId.Value == oldCustomerId;
                    var willBePrimaryForOldCustomer = willBelongToOldCustomer && newIsPrimary; 

                    var removingPrimaryFromOld = isCurrentlyPrimary && !willBePrimaryForOldCustomer; 

                    if (removingPrimaryFromOld)
                    {
                        var vehiclesOfCustomer = await _unitOfWork.Vehicles.GetByCustomerIdAsync(oldCustomerId);
                        var hasOtherPrimary = vehiclesOfCustomer
                            .Any(v => v.Id != entity.Id && v.IsPrimary);                                      

                        if (!hasOtherPrimary)
                        {
                            throw new AppException(
                                "Khách hàng phải chọn 1 xe làm xe chính",
                                HttpStatusCode.BadRequest
                            );                                                                                 
                        }
                    }
                }
                entity.Image = newImage;                                             
                entity.Color = newColor;                                             
                entity.Status = newStatus;                                            
                entity.PurchaseDate = newPurchaseDate;                               
                entity.WarrantyExpiry = newWarrantyExpiry;                           
                entity.ModelId = newModelId;
                entity.CustomerId = newCustomerId;
                if (entity.CustomerId.HasValue && newIsPrimary)
                {
                    var others = await _unitOfWork.Vehicles.GetByCustomerIdAsync(entity.CustomerId.Value);
                    foreach (var v in others.Where(x => x.Id != entity.Id && x.IsPrimary))
                    {
                        v.IsPrimary = false;
                        await _unitOfWork.Vehicles.UpdateAsync(v);
                    }
                    entity.IsPrimary = true;
                }
                else if (!entity.CustomerId.HasValue)
                {
                    entity.IsPrimary = false;
                }
                else
                {
                    entity.IsPrimary = newIsPrimary;
                }
                await _unitOfWork.Vehicles.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Vehicle {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
                var entity =
                    await _unitOfWork.Vehicles.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.NotFound);
                entity.Status = StatusEnum.IN_ACTIVE;
                await _unitOfWork.Vehicles.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Vehicle {Id} đã được vô hiệu hóa", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deactivate Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<VehicleHistoryResponse> GetHistoryAsync(Guid vehicleId)
        {
            var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(vehicleId);
            if (vehicle is null)
                throw new AppException("Không tìm thấy xe.", HttpStatusCode.NotFound);

            var appointments = await _unitOfWork.Appointments.GetByVehicleIdAsync(vehicleId);

            var appointmentDtos = _mapper.Map<List<AppointmentResponse>>(appointments);

            var maintenanceHistory = appointmentDtos
                .Where(a => a.Type == ServiceType.MAINTENANCE_TYPE)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            var repairHistory = appointmentDtos
                .Where(a => a.Type == ServiceType.REPAIR_TYPE)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            var warrantyHistory = appointmentDtos
                .Where(a => a.Type == ServiceType.WARRANTY_TYPE)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            // var campaignEntities = appointments
            //     .Where(a => a.CampaignId != null && a.Campaign != null)
            //     .GroupBy(a => a.CampaignId)
            //     .Select(g => g.First().Campaign!)
            //     .ToList();

            // var campaignDtos = _mapper.Map<List<CampaignResponse>>(campaignEntities);

            var vehiclePartItems = await _unitOfWork.VehiclePartItems.GetListByVehicleIdAsync(vehicleId);
            var replacedParts = vehiclePartItems
                .OrderByDescending(x => x.InstallDate)
                .Select(x => new VehiclePartItemHistoryResponse
                {
                    Id = x.Id,
                    InstallDate = x.InstallDate,
                    NewPartItem = _mapper.Map<PartItemResponse>(x.PartItem),
                    ReplacedForPartItem = x.ReplaceFor != null ? _mapper.Map<PartItemResponse>(x.ReplaceFor) : null,
                })
                .ToList();
            var response = new VehicleHistoryResponse
            {
                Vehicle = _mapper.Map<VehicleResponse>(vehicle),
                MaintenanceHistory = maintenanceHistory,
                RepairHistory = repairHistory,
                WarrantyHistory = warrantyHistory,
                // CampaignHistory = campaignDtos,
                ReplacedParts = replacedParts
            };

            return response;
        }

        public async Task<List<VehicleResponse>> SyncVehicleAsync()
        {
            try
            {
                if (!_firebase.IsFirestoreConfigured())
                    throw new AppException(
                        "Hệ thống chưa được cấu hình Firestore",
                        HttpStatusCode.ServiceUnavailable
                    );

                var firebaseVehicles = await _firebase.GetAllVehiclesAsync();
                var result = new List<VehicleResponse>();

                foreach (var (firebaseVehicleId, data) in firebaseVehicles)
                {
                    var chassisNumber = data.ContainsKey("chassis_number")
                        ? data["chassis_number"]?.ToString()?.Trim() ?? ""
                        : "";

                    if (string.IsNullOrWhiteSpace(chassisNumber))
                        continue; // skip bản ghi lỗi

                    // tìm trong DB
                    var existed = await _unitOfWork.Vehicles.GetByChassisNumberAsync(chassisNumber);

                    var engineNumber = data.ContainsKey("engine_number")
                        ? data["engine_number"]?.ToString()?.Trim() ?? ""
                        : "";

                    var color = data.ContainsKey("color")
                        ? data["color"]?.ToString()?.Trim() ?? ""
                        : "";

                    var image = data.ContainsKey("image")
                        ? data["image"]?.ToString()?.Trim() ?? ""
                        : "";

                    var modelIdStr = data.ContainsKey("modelId")
                        ? data["modelId"]?.ToString()
                        : null;

                    if (string.IsNullOrWhiteSpace(modelIdStr))
                        continue; 
                    // đảm bảo Model tồn tại (dùng lại SyncModelAsync)
                    var modelResp = await _modelService.SyncModelAsync(
                        new SyncModelRequest { modelIdOrName = modelIdStr }
                    );
                    var modelId = modelResp.Id;
                    Guid? customerId = null;
                    if (data.ContainsKey("customerId") &&
                        Guid.TryParse(data["customerId"]?.ToString(), out var parsedCustomerId))
                    {
                        customerId = parsedCustomerId;
                    }

                    bool isPrimary = false;

                    if (customerId.HasValue)
                    {
                        var customerVehicles =
                            await _unitOfWork.Vehicles.GetByCustomerIdAsync(customerId.Value);

                        if (!customerVehicles.Any())
                        {
                            isPrimary = true;
                        }
                        else if (!customerVehicles.Any(v => v.IsPrimary))
                        {
                            isPrimary = true;
                        }
                    }
                    if (existed == null)
                    {
                        // tạo mới
                        var vehicle = new Vehicle
                        {
                            Id = Guid.NewGuid(),
                            ChassisNumber = chassisNumber,
                            EngineNumber = engineNumber,
                            Color = color,
                            Image = image,
                            ModelId = modelId,
                            CustomerId = null,
                            IsPrimary = isPrimary,
                            Status = StatusEnum.ACTIVE,
                            ManufactureDate = DateTime.UtcNow,
                            PurchaseDate = DateTime.UtcNow,
                            WarrantyExpiry = DateTime.UtcNow,
                        };

                        await _unitOfWork.Vehicles.CreateAsync(vehicle);
                        result.Add(_mapper.Map<VehicleResponse>(vehicle));
                    }
                    else
                    {
                        // update theo dữ liệu mới nhất từ OEM
                        existed.EngineNumber = engineNumber;
                        existed.Color = color;
                        existed.Image = image;
                        existed.ModelId = modelId;

                        await _unitOfWork.Vehicles.UpdateAsync(existed);
                        result.Add(_mapper.Map<VehicleResponse>(existed));
                    }
                }

                await _unitOfWork.SaveAsync();
                return result;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sync Vehicle failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
