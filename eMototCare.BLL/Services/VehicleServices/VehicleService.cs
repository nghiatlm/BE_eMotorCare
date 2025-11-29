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

        public async Task UpdateAsync(Guid id, VehicleRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Vehicles.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy xe", HttpStatusCode.NotFound);
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
                _mapper.Map(req, entity);
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

            var response = new VehicleHistoryResponse
            {
                Vehicle = _mapper.Map<VehicleResponse>(vehicle),
                MaintenanceHistory = maintenanceHistory,
                RepairHistory = repairHistory,
                WarrantyHistory = warrantyHistory,
                // CampaignHistory = campaignDtos,
            };

            return response;
        }

        public async Task<VehicleResponse> SyncVehicleAsync(SyncVehicleRequest request)
        {
            try
            {
                if (request == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                var chassisNumber = request.ChassisNumber?.Trim();
                if (string.IsNullOrWhiteSpace(chassisNumber))
                    throw new AppException(
                        "ChassisNumber không được để trống",
                        HttpStatusCode.BadRequest
                    );

                if (!_firebase.IsFirestoreConfigured())
                    throw new AppException(
                        "Hệ thống chưa được cấu hình Firestore",
                        HttpStatusCode.ServiceUnavailable
                    );

                // 1. Kiểm tra trong DB trước
                var existed = await _unitOfWork.Vehicles.GetByChassisNumberAsync(chassisNumber);
                if (existed != null)
                    return _mapper.Map<VehicleResponse>(existed);

                // 2. Không có trong DB -> lấy từ Firestore theo chassisNumber
                var vehicleTuple = await _firebase.GetVehicleByChassisNumberAsync(chassisNumber);
                if (vehicleTuple == null)
                    throw new AppException(
                        "Không tìm thấy vehicle trong hệ thống OEM",
                        HttpStatusCode.NotFound
                    );

                var firebaseVehicleId = vehicleTuple.Value.Id;
                var data = vehicleTuple.Value.Data;

                // 3. Đọc dữ liệu từ Firestore
                var fsChassis = data.ContainsKey("chassis_number")
                    ? data["chassis_number"]?.ToString()?.Trim() ?? ""
                    : "";

                var engineNumber = data.ContainsKey("engine_number")
                    ? data["engine_number"]?.ToString()?.Trim() ?? ""
                    : "";

                var color = data.ContainsKey("color")
                    ? data["color"]?.ToString()?.Trim() ?? ""
                    : "";

                var image = data.ContainsKey("image")
                    ? data["image"]?.ToString()?.Trim() ?? ""
                    : "";

                var modelIdStr = data.ContainsKey("modelId") ? data["modelId"]?.ToString() : null;

                if (string.IsNullOrWhiteSpace(fsChassis))
                    throw new AppException(
                        "Dữ liệu OEM thiếu chassis_number",
                        HttpStatusCode.BadRequest
                    );

                // nếu OEM trả chassis khác thì vẫn ưu tiên theo request
                var finalChassis = chassisNumber;

                // 4. Đảm bảo Model tồn tại (tái sử dụng SyncModelAsync)
                if (string.IsNullOrWhiteSpace(modelIdStr))
                    throw new AppException(
                        "Dữ liệu OEM không có modelId",
                        HttpStatusCode.BadRequest
                    );

                var modelResp = await _modelService.SyncModelAsync(
                    new SyncModelRequest { modelIdOrName = modelIdStr }
                );
                var modelId = modelResp.Id;

                // 5. (tuỳ bạn) hiện tại chưa map CustomerId -> để null
                Guid? customerId = null;

                // 6. Tạo Vehicle mới trong DB (map đủ engine_number, color, image)
                var vehicle = new Vehicle
                {
                    Id = Guid.NewGuid(),
                    ChassisNumber = finalChassis,
                    EngineNumber = engineNumber, // đổi tên property cho khớp entity nếu khác
                    Color = color,
                    Image = image, // hoặc ImageUrl, Avatar, ...
                    ModelId = modelId,
                    CustomerId = customerId,
                    // nếu có Status / CreatedAt / UpdatedAt thì set thêm:
                    // Status = VehicleStatus.ACTIVE,
                    // CreatedAt = DateTime.UtcNow,
                };

                await _unitOfWork.Vehicles.CreateAsync(vehicle);
                await _unitOfWork.SaveAsync();

                // 7. (optional) đồng bộ luôn VehicleStage nếu cần – để sau cũng được
                // var stagesData = await _firebase.GetVehicleStagesByVehicleIdAsync(firebaseVehicleId);
                // ...

                return _mapper.Map<VehicleResponse>(vehicle);
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
