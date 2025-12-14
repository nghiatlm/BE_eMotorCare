using System.Net;
using System.Security.Claims;
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
using eMototCare.BLL.Services.VehicleServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.AppointmentServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFirebaseService _firebase;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AppointmentService> logger,
            IHttpContextAccessor httpContextAccessor,
            IFirebaseService firebase
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _firebase = firebase;
        }

        public async Task<PageResult<AppointmentResponse>> GetPagedAsync(
            string? search,
            AppointmentStatus? status,
            Guid? serviceCenterId,
            Guid? customerId,
            Guid? technicianId,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Appointments.GetPagedAsync(
                    search,
                    status,
                    serviceCenterId,
                    customerId,
                    technicianId,
                    fromDate,
                    toDate,
                    page,
                    pageSize
                );
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (status.HasValue && !Enum.IsDefined(typeof(AppointmentStatus), status.Value))
                    throw new AppException(
                        "Trạng thái lịch hẹn không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (serviceCenterId.HasValue && serviceCenterId.Value == Guid.Empty)
                    throw new AppException(
                        "ServiceCenterId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (customerId.HasValue && customerId.Value == Guid.Empty)
                    throw new AppException("CustomerId không hợp lệ", HttpStatusCode.BadRequest);

                if (fromDate.HasValue && toDate.HasValue && fromDate > toDate)
                    throw new AppException(
                        "fromDate không được > toDate",
                        HttpStatusCode.BadRequest
                    );

                var rows = _mapper.Map<List<AppointmentResponse>>(items);
                foreach (var appt in rows)
                {
                    var evCheck = await _unitOfWork.EVChecks.GetByAppointmentIdAsync(appt.Id);
                    if (evCheck != null)
                        appt.EVCheckId = evCheck.Id;
                }
                return new PageResult<AppointmentResponse>(rows, pageSize, page, (int)total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Appointment failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<AppointmentResponse?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
            var appt =
                await _unitOfWork.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            return _mapper.Map<AppointmentResponse>(appt);
        }

        public async Task<Guid> CreateAsync(AppointmentRequest req)
        {
            try
            {
                var dateOnly = DateOnly.FromDateTime(req.AppointmentDate.Date);
                var dow = (DayOfWeeks)req.AppointmentDate.DayOfWeek;

                var now = DateTime.UtcNow.AddHours(7).Date;
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (req.ServiceCenterId == Guid.Empty)
                    throw new AppException(
                        "ServiceCenterId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (req.CustomerId == Guid.Empty)
                    throw new AppException("CustomerId không hợp lệ", HttpStatusCode.BadRequest);
                if (!Enum.IsDefined(typeof(AppointmentStatus), req.Status))
                    throw new AppException(
                        "Trạng thái lịch hẹn không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (!Enum.IsDefined(typeof(ServiceType), req.Type))
                    throw new AppException("Loại dịch vụ không hợp lệ", HttpStatusCode.BadRequest);
                if (req.EstimatedCost.HasValue && req.EstimatedCost.Value < 0)
                    throw new AppException(
                        "EstimatedCost không được âm",
                        HttpStatusCode.BadRequest
                    );
                if (req.AppointmentDate == default)
                    throw new AppException("Ngày hẹn không hợp lệ", HttpStatusCode.BadRequest);

                if (req.ActualCost.HasValue && req.ActualCost.Value < 0)
                    throw new AppException("ActualCost không được âm", HttpStatusCode.BadRequest);

                if (req.VehicleId.HasValue && req.VehicleId.Value == Guid.Empty)
                    throw new AppException("VehicleId không hợp lệ", HttpStatusCode.BadRequest);

                if (req.VehicleStageId.HasValue && req.VehicleStageId.Value == Guid.Empty)
                    throw new AppException(
                        "VehicleStageId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                if (req.AppointmentDate.Date < now)
                {
                    throw new AppException("Ngày đặt phải từ hôm nay trở đi.");
                }
                if (req.RmaId.HasValue)
                {
                    var rmaId = req.RmaId.Value;

                    var rma = await _unitOfWork.RMAs.GetByIdAsync(rmaId);
                    if (rma is null)
                        throw new AppException(
                            "Không tìm thấy phiếu RMA.",
                            HttpStatusCode.BadRequest
                        );

                    if (rma.Status == RMAStatus.PENDING)
                        throw new AppException(
                            "Phiếu RMA đang chờ duyệt, không thể đặt lịch hẹn.",
                            HttpStatusCode.Conflict
                        );

                    if (rma.Status == RMAStatus.APPOINTMENT_BOOKED)
                        throw new AppException(
                            "Phiếu RMA này đã có lịch hẹn, không thể đặt lịch tiếp tục.",
                            HttpStatusCode.Conflict
                        );
                }
                if (req.Type == ServiceType.MAINTENANCE_TYPE)
                {
                    if (!req.VehicleStageId.HasValue)
                        throw new AppException(
                            "Lịch bảo dưỡng phải chọn mốc bảo dưỡng (VehicleStage).",
                            HttpStatusCode.BadRequest
                        );
                }
                else
                {
                    if (!req.VehicleId.HasValue)
                        throw new AppException(
                            "Lịch sửa chữa/bảo hành phải chọn xe (Vehicle).",
                            HttpStatusCode.BadRequest
                        );

                    req.VehicleStageId = null;
                }
                if (req.Type == ServiceType.CAMPAIGN_TYPE)
                {
                    if (!req.CampaignId.HasValue || req.CampaignId.Value == Guid.Empty)
                        throw new AppException(
                            "Lịch campaign phải chọn CampaignId.",
                            HttpStatusCode.BadRequest
                        );

                    if (!req.VehicleId.HasValue || req.VehicleId.Value == Guid.Empty)
                        throw new AppException(
                            "Lịch campaign phải gắn với một Vehicle.",
                            HttpStatusCode.BadRequest
                        );

                    var allAppointmentsForCampaign = await _unitOfWork.Appointments.FindAllAsync();
                    var existedCampaignForVehicle = allAppointmentsForCampaign.Any(a =>
                        a.CampaignId == req.CampaignId.Value
                        && a.VehicleId == req.VehicleId.Value
                        && a.Status != AppointmentStatus.CANCELED
                    );

                    if (existedCampaignForVehicle)
                    {
                        throw new AppException(
                            "Xe này đã được đặt lịch cho campaign này rồi, không thể đặt thêm.",
                            HttpStatusCode.Conflict
                        );
                    }
                }
                VehicleStage? stage = null;
                if (req.VehicleStageId.HasValue)
                {
                    stage = await _unitOfWork.VehicleStages.GetByIdAsync(req.VehicleStageId.Value);
                    if (stage is null)
                        throw new AppException(
                            "Mốc bảo dưỡng không tồn tại.",
                            HttpStatusCode.BadRequest
                        );

                    switch (stage.Status)
                    {
                        case VehicleStageStatus.UPCOMING:
                            break;

                        case VehicleStageStatus.COMPLETED:
                            throw new AppException(
                                "Mốc bảo dưỡng này đã hoàn thành.",
                                HttpStatusCode.BadRequest
                            );

                        case VehicleStageStatus.EXPIRED:
                            throw new AppException(
                                "Mốc bảo dưỡng này đã hết hạn.",
                                HttpStatusCode.BadRequest
                            );

                        case VehicleStageStatus.NO_START:
                            throw new AppException(
                                "Mốc bảo dưỡng này chưa đến thời điểm bắt đầu.",
                                HttpStatusCode.BadRequest
                            );
                    }

                    var allAppointments = await _unitOfWork.Appointments.FindAllAsync();
                    var existed = allAppointments.Any(a =>
                        a.VehicleStageId == req.VehicleStageId.Value
                        && a.Status != AppointmentStatus.CANCELED
                    );

                    if (existed)
                        throw new AppException(
                            "Mốc bảo dưỡng này đã có đặt lịch.",
                            HttpStatusCode.BadRequest
                        );
                }

                var slotCfg = (await _unitOfWork.ServiceCenterSlot.FindAllAsync()).FirstOrDefault(
                    s =>
                        s.ServiceCenterId == req.ServiceCenterId
                        //&& s.IsActive
                        && (s.Date == dateOnly || (s.Date == default && s.DayOfWeek == dow))
                        && s.SlotTime == req.SlotTime
                );
                if (slotCfg is null)
                    throw new AppException(
                        "Ngày này không có khung giờ đó.",
                        HttpStatusCode.Conflict
                    );

                // Đếm appointment
                var booked = await _unitOfWork.ServiceCenterSlot.CountBookingsAsync(
                    req.ServiceCenterId,
                    dateOnly,
                    req.SlotTime
                );
                if (booked >= slotCfg.Capacity)
                    throw new AppException("Khung giờ này đã đầy.", HttpStatusCode.Conflict);
                var remaining = slotCfg.Capacity - booked;

                if (!slotCfg.IsActive || remaining <= 0)
                {
                    throw new AppException(
                        "Mốc thời gian này đã quá hạn hoặc đã không còn chỗ trống",
                        HttpStatusCode.BadRequest
                    );
                }

                string code;
                int guard = 0;
                do
                {
                    code = $"APPT-{req.AppointmentDate:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                    guard++;
                } while (await _unitOfWork.Appointments.ExistsCodeAsync(code) && guard < 5);

                var entity = _mapper.Map<Appointment>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = code;

                await _unitOfWork.Appointments.CreateAsync(entity);
                if (stage != null)
                {
                    stage.ExpectedImplementationDate = req.AppointmentDate;
                    await _unitOfWork.VehicleStages.UpdateAsync(stage);
                }
                await _unitOfWork.SaveAsync();
                if (req.RmaId.HasValue)
                {
                    var rma = await _unitOfWork.RMAs.GetByIdAsync(req.RmaId.Value);
                    if (rma != null && rma.Status != RMAStatus.APPOINTMENT_BOOKED)
                    {
                        rma.Status = RMAStatus.APPOINTMENT_BOOKED;
                        await _unitOfWork.RMAs.UpdateAsync(rma);
                        await _unitOfWork.SaveAsync();
                    }
                }
                var current = await _unitOfWork.ServiceCenterSlot.CountBookingsAsync(
                    req.ServiceCenterId,
                    dateOnly,
                    req.SlotTime
                );

                if (current >= slotCfg.Capacity && slotCfg.IsActive)
                {
                    slotCfg.IsActive = false;
                    await _unitOfWork.ServiceCenterSlot.UpdateAsync(slotCfg);
                    await _unitOfWork.SaveAsync();
                    _logger.LogInformation(
                        "Slot {SlotId} đã đủ số lượng, tự động đóng.",
                        slotCfg.Id
                    );
                }
                _logger.LogInformation("Created Appointment {Code} ({Id})", entity.Code, entity.Id);
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Appointment failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, AppointmentUpdateRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Appointments.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);
                var oldStatus = entity.Status;
                var httpUser = _httpContextAccessor.HttpContext?.User;
                var roleClaim = httpUser?.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.Role || c.Type == "role" || c.Type == "roles"
                );
                var currentRole = roleClaim?.Value;
                if (req.SlotTime.HasValue && entity.SlotTime != req.SlotTime.Value)
                {
                    var dateOnly = DateOnly.FromDateTime(entity.AppointmentDate.Date);
                    var dow = (DayOfWeeks)entity.AppointmentDate.DayOfWeek;

                    var slotCfg = (
                        await _unitOfWork.ServiceCenterSlot.FindAllAsync()
                    ).FirstOrDefault(s =>
                        s.ServiceCenterId == entity.ServiceCenterId
                        && s.IsActive
                        && (s.Date == dateOnly || (s.Date == default && s.DayOfWeek == dow))
                        && s.SlotTime == req.SlotTime.Value
                    );

                    if (slotCfg is null)
                        throw new AppException(
                            "Ngày này không có khung giờ đó.",
                            HttpStatusCode.Conflict
                        );

                    var booked = await _unitOfWork.ServiceCenterSlot.CountBookingsAsync(
                        entity.ServiceCenterId,
                        dateOnly,
                        req.SlotTime.Value
                    );

                    if (booked >= slotCfg.Capacity)
                        throw new AppException("Khung giờ này đã đầy.", HttpStatusCode.Conflict);

                    entity.SlotTime = req.SlotTime.Value;
                }
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);

                if (!Enum.IsDefined(typeof(AppointmentStatus), req.Status))
                    throw new AppException(
                        "Trạng thái lịch hẹn không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (req.SlotTime.HasValue && !Enum.IsDefined(typeof(SlotTime), req.SlotTime.Value))
                    throw new AppException("SlotTime không hợp lệ", HttpStatusCode.BadRequest);

                if (req.EstimatedCost.HasValue && req.EstimatedCost.Value < 0)
                    throw new AppException(
                        "EstimatedCost không được âm",
                        HttpStatusCode.BadRequest
                    );

                if (req.ActualCost.HasValue && req.ActualCost.Value < 0)
                    throw new AppException("ActualCost không được âm", HttpStatusCode.BadRequest);

                if (req.ApproveById.HasValue && req.ApproveById.Value == Guid.Empty)
                    throw new AppException("ApproveById không hợp lệ", HttpStatusCode.BadRequest);
                if (req.EstimatedCost.HasValue)
                    entity.EstimatedCost = req.EstimatedCost.Value;

                if (req.ActualCost.HasValue)
                    entity.ActualCost = req.ActualCost.Value;

                if (req.Note != null)
                    entity.Note = req.Note;
                if (req.Status == AppointmentStatus.APPROVED && currentRole == "ROLE_CUSTOMER")
                {
                    throw new AppException(
                        "Khách hàng không được phép duyệt lịch hẹn.",
                        HttpStatusCode.Forbidden
                    );
                }
                VehicleStage? relatedStage = entity.VehicleStage;
                if (oldStatus != req.Status)
                {
                    switch (req.Status)
                    {
                        case AppointmentStatus.APPROVED:
                            if (oldStatus != AppointmentStatus.PENDING)
                                throw new AppException(
                                    "Chỉ lịch hẹn PENDING mới được duyệt.",
                                    HttpStatusCode.Conflict
                                );

                            if (!req.ApproveById.HasValue)
                                throw new AppException(
                                    "Thiếu thông tin nhân viên duyệt.",
                                    HttpStatusCode.BadRequest
                                );

                            if (string.IsNullOrWhiteSpace(req.CheckinQRCode))
                                throw new AppException(
                                    "Mã check-in không hợp lệ.",
                                    HttpStatusCode.BadRequest
                                );

                            entity.ApproveById = req.ApproveById.Value;
                            entity.CheckinQRCode = req.CheckinQRCode;
                            break;

                        case AppointmentStatus.CHECKED_IN:

                            if (oldStatus != AppointmentStatus.APPROVED)
                                throw new AppException(
                                    "Chỉ lịch hẹn APPROVED mới được check-in.",
                                    HttpStatusCode.Conflict
                                );

                            if (string.IsNullOrWhiteSpace(req.CheckinQRCode))
                                throw new AppException(
                                    "Thiếu đường dẫn QR check-in.",
                                    HttpStatusCode.BadRequest
                                );

                            if (string.IsNullOrWhiteSpace(entity.CheckinQRCode))
                                throw new AppException(
                                    "Lịch hẹn chưa có QR check-in được lưu.",
                                    HttpStatusCode.Conflict
                                );
                            entity.CheckedInAt = DateTime.UtcNow.AddHours(7);
                            if (
                                !string.Equals(
                                    req.CheckinQRCode.Trim(),
                                    entity.CheckinQRCode.Trim(),
                                    StringComparison.Ordinal
                                )
                            )
                            {
                                throw new AppException(
                                    "QR check-in không khớp.",
                                    HttpStatusCode.Conflict
                                );
                            }
                            if (
                                relatedStage != null
                                && !relatedStage.ActualImplementationDate.HasValue
                            )
                            {
                                relatedStage.ActualImplementationDate = DateTime.UtcNow.AddHours(7);
                            }
                            if (
                                !string.IsNullOrWhiteSpace(req.Code)
                                && !string.Equals(
                                    req.Code.Trim(),
                                    entity.Code.Trim(),
                                    StringComparison.OrdinalIgnoreCase
                                )
                            )
                            {
                                throw new AppException(
                                    "Mã lịch hẹn không khớp với QR.",
                                    HttpStatusCode.Conflict
                                );
                            }
                            
                            break;

                        case AppointmentStatus.COMPLETED:
                            if (
                                relatedStage != null
                                && relatedStage.Status != VehicleStageStatus.COMPLETED
                            )
                            {
                                relatedStage.Status = VehicleStageStatus.COMPLETED;
                                if (!relatedStage.ActualImplementationDate.HasValue)
                                {
                                    relatedStage.ActualImplementationDate = DateTime.UtcNow;
                                }

                                _logger.LogInformation(
                                    "VehicleStage {StageId} -> COMPLETED (từ Appointment {Id})",
                                    relatedStage.Id,
                                    entity.Id
                                );
                            }
                            break;
                        default:
                            break;
                    }

                    entity.Status = req.Status;
                }

                await _unitOfWork.Appointments.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Updated Appointment {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Appointment failed: {Message}", ex.Message);
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
                    await _unitOfWork.Appointments.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

                await _unitOfWork.Appointments.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Appointment {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Appointment failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public Task<IReadOnlyList<string>> GetAvailableSlotsAsync(
            Guid serviceCenterId,
            DateTime date
        )
        {
            try
            {
                if (serviceCenterId == Guid.Empty)
                    throw new AppException(
                        "ServiceCenterId không hợp lệ",
                        HttpStatusCode.BadRequest
                    );

                if (date == default)
                    throw new AppException("Ngày không hợp lệ", HttpStatusCode.BadRequest);
                return _unitOfWork.Appointments.GetAvailableSlotsAsync(serviceCenterId, date);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAvailableSlots failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateStatusAsync(Guid id, AppointmentStatus status)
        {
            if (id == Guid.Empty)
                throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

            if (!Enum.IsDefined(typeof(AppointmentStatus), status))
                throw new AppException(
                    "Trạng thái lịch hẹn không hợp lệ",
                    HttpStatusCode.BadRequest
                );
            var entity =
                await _unitOfWork.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            entity.Status = status;
            await _unitOfWork.Appointments.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();
        }
        private static string? GetStr(IDictionary<string, object> dict, params string[] keys)
        {
            foreach (var k in keys)
                if (dict.TryGetValue(k, out var obj) && obj != null)
                    return obj.ToString();
            return null;
        }

        private static DateTime? TryParseDate(IDictionary<string, object> dict, string key)
        {
            var s = GetStr(dict, key);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }
        private async Task<List<VehicleStage>> SyncVehicleStagesFromOemAsync(Guid vehicleId, string vehicleDocId)
        {
            var stageDataList = await _firebase.GetVehicleStagesByVehicleIdAsync(vehicleDocId);
            var vehicleStages = new List<VehicleStage>();

            foreach (var sd in stageDataList)
            {
                Console.WriteLine("[DEBUG] VehicleStage from OEM: " +
                                  string.Join(", ", sd.Select(kv => $"{kv.Key}={kv.Value}")));

                var msIdStr = GetStr(sd, "maintenanceStageId", "maintenance_stage_id", "maintenancestageId");
                if (string.IsNullOrWhiteSpace(msIdStr) || !Guid.TryParse(msIdStr, out var msId))
                {
                    Console.WriteLine($"[WARN] maintenanceStageId invalid or missing: {msIdStr}");
                    continue;
                }

                var msEntity = await _unitOfWork.MaintenanceStages.GetByIdAsync(msId);

                if (msEntity == null)
                {
                    var msData = await _firebase.GetMaintenanceStageByIdAsync(msIdStr);
                    if (msData == null)
                    {
                        Console.WriteLine("[WARN] maintenanceStage data not found on OEM, skip.");
                        continue;
                    }

                    var mpIdStr = GetStr(msData, "maintenancePlanId", "maintenance_plan_id", "maintenance_plan_id ");
                    if (string.IsNullOrWhiteSpace(mpIdStr) || !Guid.TryParse(mpIdStr, out var mpId))
                    {
                        Console.WriteLine("[WARN] maintenancePlanId not found for MaintenanceStage, skip.");
                        continue;
                    }

                    var mpEntity = await _unitOfWork.MaintenancePlans.GetByIdAsync(mpId);
                    if (mpEntity == null)
                    {
                        var mpCode = GetStr(msData, "planCode") ?? $"OEM-{mpId.ToString()[..8]}";
                        var mpName = GetStr(msData, "planName") ?? "OEM Maintenance Plan";
                        var mpDesc = GetStr(msData, "description") ?? "";
                        var unitStr = GetStr(msData, "unit");
                        var effStr = GetStr(msData, "effectiveDate");

                        var unit = MaintenanceUnit.KILOMETER;
                        if (!string.IsNullOrWhiteSpace(unitStr)
                            && Enum.TryParse<MaintenanceUnit>(unitStr, true, out var parsedUnit))
                            unit = parsedUnit;

                        var effDate = DateTime.UtcNow;
                        if (!string.IsNullOrWhiteSpace(effStr)
                            && DateTime.TryParse(effStr, out var effParsed))
                            effDate = effParsed;

                        mpEntity = new MaintenancePlan
                        {
                            Id = mpId,
                            Code = mpCode,
                            Name = mpName,
                            Description = mpDesc,
                            Status = Status.ACTIVE,
                            Unit = new[] { unit },
                            EffectiveDate = effDate,
                            TotalStages = 0
                        };

                        await _unitOfWork.MaintenancePlans.CreateAsync(mpEntity);
                    }

                    var name = (GetStr(msData, "name") ?? "").Trim();
                    var desc = GetStr(msData, "description") ?? "";
                    var mileageStr = GetStr(msData, "mileage") ?? "";
                    var msStatusStr = GetStr(msData, "status");
                    var durationStr = GetStr(msData, "duration_month");   
                    var estTimeStr = GetStr(msData, "estimated_time");
                    var msStatus = StatusEnum.ACTIVE;
                    if (!string.IsNullOrWhiteSpace(msStatusStr)
                        && Enum.TryParse<StatusEnum>(msStatusStr, true, out var stParsed))
                        msStatus = stParsed;
                    DurationMonth durationMonth = DurationMonth.MONTH_3;
                    if (!string.IsNullOrWhiteSpace(durationStr)
                        && Enum.TryParse<DurationMonth>(durationStr, true, out var dmParsed))
                    {
                        durationMonth = dmParsed;
                    }

                    // parse EstimatedTime (nếu cần)
                    int? estimatedTime = null;
                    if (!string.IsNullOrWhiteSpace(estTimeStr)
                        && int.TryParse(estTimeStr, out var et))
                    {
                        estimatedTime = et;
                    }
                    if (!Enum.TryParse<Mileage>(mileageStr, true, out var parsedMileage))
                        throw new AppException("Invalid mileage value.", HttpStatusCode.BadRequest);

                    msEntity = new MaintenanceStage
                    {
                        Id = msId,
                        MaintenancePlanId = mpEntity.Id,
                        Name = name,
                        Description = desc,
                        Mileage = parsedMileage,
                        Status = (Status)msStatus,
                        DurationMonth = durationMonth,     
                        EstimatedTime = estimatedTime,
                    };

                    await _unitOfWork.MaintenanceStages.CreateAsync(msEntity);
                }

                if (msEntity == null)
                {
                    Console.WriteLine("[WARN] Cannot resolve MaintenanceStage entity, skip VehicleStage.");
                    continue;
                }
                var statusStr = GetStr(sd, "status");
                var vsStatus = VehicleStageStatus.UPCOMING;
                if (!string.IsNullOrWhiteSpace(statusStr)
                    && Enum.TryParse<VehicleStageStatus>(statusStr, true, out var vsParsed))
                    vsStatus = vsParsed;

                var implDate =
                    TryParseDate(sd, "expectedImplementationDate")
                    ?? TryParseDate(sd, "expectedStartDate")
                    ?? DateTime.UtcNow;

                var vs = new VehicleStage
                {
                    Id = Guid.NewGuid(),
                    VehicleId = vehicleId,
                    MaintenanceStageId = msEntity.Id,
                    ActualMaintenanceMileage = 0,
                    ActualMaintenanceUnit = MaintenanceUnit.KILOMETER,
                    ExpectedStartDate = TryParseDate(sd, "expectedStartDate") ?? implDate,
                    ExpectedEndDate = TryParseDate(sd, "expectedEndDate"),
                    ExpectedImplementationDate = implDate,
                    ActualImplementationDate = null,
                    Status = vsStatus
                };

                await _unitOfWork.VehicleStages.CreateAsync(vs);
                vehicleStages.Add(vs);
            }

            return vehicleStages;
        }

        private async Task<(Customer Customer, Vehicle Vehicle, List<VehicleStage> VehicleStages)>
            EnsureVehicleFromVinAsync(string chassisNumber, Guid? accountId)
        {
            if (string.IsNullOrWhiteSpace(chassisNumber))
                throw new AppException("ChassisNumber không được để trống", HttpStatusCode.BadRequest);

            var localVehicle = await _unitOfWork.Vehicles.GetByChassisNumberAsync(chassisNumber);
            if (localVehicle != null)
            {
                List<VehicleStage> stages;

                if (_firebase.IsFirestoreConfigured())
                {
                    var fireVehicleInner = await _firebase.GetVehicleByChassisNumberAsync(chassisNumber);

                    if (fireVehicleInner != null)
                    {
                        var vDataRemote = fireVehicleInner.Value.Data;
                        var vehicleDocId = fireVehicleInner.Value.Id;
                        localVehicle.Image = GetStr(vDataRemote, "image") ?? localVehicle.Image;
                        localVehicle.Color = GetStr(vDataRemote, "color") ?? localVehicle.Color;
                        localVehicle.EngineNumber = GetStr(vDataRemote, "engine_number") ?? localVehicle.EngineNumber;
                        localVehicle.ManufactureDate = TryParseDate(vDataRemote, "manufacture_date") ?? localVehicle.ManufactureDate;
                        localVehicle.PurchaseDate = TryParseDate(vDataRemote, "purchase_date") ?? localVehicle.PurchaseDate;
                        localVehicle.WarrantyExpiry = TryParseDate(vDataRemote, "warranty_expiry") ?? localVehicle.WarrantyExpiry;
                        var isPrimaryStrRemote = GetStr(vDataRemote, "is_primary");
                        if (!string.IsNullOrWhiteSpace(isPrimaryStrRemote)
                            && bool.TryParse(isPrimaryStrRemote, out var isPrimaryRemote))
                        {
                            localVehicle.IsPrimary = isPrimaryRemote;
                        }

                        await _unitOfWork.Vehicles.UpdateAsync(localVehicle);

                        var oldStages = await _unitOfWork.VehicleStages.GetByVehicleIdAsync(localVehicle.Id);
                        foreach (var s in oldStages)
                            await _unitOfWork.VehicleStages.DeleteAsync(s);

                        stages = await SyncVehicleStagesFromOemAsync(localVehicle.Id, vehicleDocId);
                    }
                    else
                    {
                        stages = (await _unitOfWork.VehicleStages.GetByVehicleIdAsync(localVehicle.Id)).ToList();
                    }
                }
                else
                {
                    stages = (await _unitOfWork.VehicleStages.GetByVehicleIdAsync(localVehicle.Id)).ToList();
                }

                await _unitOfWork.SaveAsync();

                var customer =
                    localVehicle.Customer
                    ?? (localVehicle.CustomerId.HasValue
                        ? await _unitOfWork.Customers.GetByIdAsync(localVehicle.CustomerId.Value)
                        : throw new AppException("Customer ID is null", HttpStatusCode.BadRequest));

                return (customer, localVehicle, stages);
            }
            if (!_firebase.IsFirestoreConfigured())
                throw new AppException("Không thể kết nối OEM (Firestore)", HttpStatusCode.InternalServerError);

            var fireVehicle = await _firebase.GetVehicleByChassisNumberAsync(chassisNumber);
            if (fireVehicle is null)
                throw new AppException("Không tìm thấy xe trong hệ thống OEM", HttpStatusCode.NotFound);

            var vData = fireVehicle.Value.Data;
            var vehicleDocIdNew = fireVehicle.Value.Id;
            var oemModelId = GetStr(vData, "modelId", "model_id");
            Model? modelEntity = null;

            if (!string.IsNullOrWhiteSpace(oemModelId))
            {
                var modelData = await _firebase.GetModelByIdAsync(oemModelId);
                if (modelData != null)
                {
                    var modelName = GetStr(modelData, "name", "Name") ?? "";
                    var modelCode = GetStr(modelData, "code", "Code") ?? "";
                    var manufacturer = GetStr(modelData, "manufacturer")?.Trim() ?? "";
                    var statusStr = GetStr(modelData, "status", "Status");
                    var status = StatusEnum.ACTIVE;

                    if (!string.IsNullOrWhiteSpace(statusStr)
                        && Enum.TryParse<StatusEnum>(statusStr, true, out var parsedStatus))
                        status = parsedStatus;

                    var maintenancePlanIdStr = GetStr(modelData, "maintenancePlanId");

                    if (string.IsNullOrWhiteSpace(modelName))
                        throw new AppException("Tên model từ OEM không hợp lệ", HttpStatusCode.BadRequest);
                    if (string.IsNullOrWhiteSpace(manufacturer))
                        throw new AppException("Hãng sản xuất từ OEM không hợp lệ", HttpStatusCode.BadRequest);
                    if (!Guid.TryParse(maintenancePlanIdStr, out var maintenancePlanId))
                        throw new AppException("MaintenancePlanId từ OEM không hợp lệ", HttpStatusCode.BadRequest);

                    var plan = await _unitOfWork.MaintenancePlans.GetByIdAsync(maintenancePlanId);
                    if (plan == null)
                    {
                        var planData = await _firebase.GetMaintenancePlanByIdAsync(maintenancePlanIdStr!);
                        if (planData == null)
                            throw new AppException("Không tìm thấy MaintenancePlan trên hệ thống OEM", HttpStatusCode.BadRequest);

                        var planCode = GetStr(planData, "code")?.Trim() ?? "";
                        var planName = GetStr(planData, "name")?.Trim() ?? "";
                        var planDescription = GetStr(planData, "description") ?? "";
                        var planStatusStr = GetStr(planData, "status");
                        var unitStr = GetStr(planData, "unit")?.Trim();
                        var totalStagesStr = GetStr(planData, "totalStages");
                        var effStr = GetStr(planData, "effectiveDate");

                        var planStatus = Status.ACTIVE;
                        if (!string.IsNullOrWhiteSpace(planStatusStr)
                            && Enum.TryParse<Status>(planStatusStr, true, out var st))
                            planStatus = st;

                        if (string.IsNullOrWhiteSpace(unitStr))
                            unitStr = "KILOMETER";

                        int totalStages = 0;
                        if (!string.IsNullOrWhiteSpace(totalStagesStr))
                            int.TryParse(totalStagesStr, out totalStages);

                        DateTime? effectiveDate = null;
                        if (!string.IsNullOrWhiteSpace(effStr)
                            && DateTime.TryParse(effStr, out var eff))
                            effectiveDate = eff;

                        var unitEnum = Enum.TryParse<MaintenanceUnit>(unitStr, true, out var parsedUnit)
                            ? parsedUnit
                            : MaintenanceUnit.KILOMETER;

                        plan = new MaintenancePlan
                        {
                            Id = maintenancePlanId,
                            Code = planCode,
                            Name = planName,
                            Description = planDescription,
                            Status = planStatus,
                            Unit = new[] { unitEnum },
                            TotalStages = totalStages,
                            EffectiveDate = effectiveDate ?? DateTime.MinValue,
                        };

                        await _unitOfWork.MaintenancePlans.CreateAsync(plan);
                    }

                    var allModels = await _unitOfWork.Models.FindAllAsync();
                    modelEntity = allModels.FirstOrDefault(m =>
                        (!string.IsNullOrEmpty(modelCode) && m.Code == modelCode)
                        || (!string.IsNullOrEmpty(modelName) && m.Name == modelName));

                    if (modelEntity == null)
                    {
                        var finalCode = string.IsNullOrWhiteSpace(modelCode)
                            ? $"MDL-{Guid.NewGuid().ToString("N")[..5].ToUpper()}"
                            : modelCode;

                        modelEntity = new Model
                        {
                            Id = Guid.NewGuid(),
                            Name = modelName,
                            Code = finalCode,
                            Manufacturer = manufacturer,
                            MaintenancePlanId = plan.Id,
                            Status = (Status)status,
                        };

                        await _unitOfWork.Models.CreateAsync(modelEntity);
                    }
                }
            }

            if (modelEntity == null)
                throw new AppException("Không sync được Model tương ứng với xe OEM", HttpStatusCode.BadRequest);
            if (!vData.TryGetValue("customerId", out var custIdObj) || custIdObj == null)
                throw new AppException("Xe trong OEM không có thông tin khách hàng", HttpStatusCode.BadRequest);

            var custData = await _firebase.GetCustomerByIdAsync(custIdObj.ToString()!);
            if (custData == null)
                throw new AppException("Không sync được khách hàng từ OEM", HttpStatusCode.BadRequest);

            var citizenId = GetStr(custData, "citizenId") ?? "";
            var firstName = GetStr(custData, "firstName") ?? "";
            var lastName = GetStr(custData, "lastName") ?? "";
            var address = GetStr(custData, "address") ?? "";
            var customerCode = GetStr(custData, "customerCode") ?? "";
            var avatarUrl = GetStr(custData, "avatarUrl") ?? "";
            var phone = GetStr(custData, "phone") ?? "";
            var emailRaw = GetStr(custData, "email");
            var email = string.IsNullOrWhiteSpace(emailRaw) ? null : emailRaw.Trim().ToLowerInvariant();
            var genderStr = GetStr(custData, "gender");
            var dobStr = GetStr(custData, "dateOfBirth");

            GenderEnum? gender = null;
            if (!string.IsNullOrWhiteSpace(genderStr)
                && Enum.TryParse<GenderEnum>(genderStr, true, out var g))
                gender = g;

            DateTime? dob = null;
            if (!string.IsNullOrWhiteSpace(dobStr) && DateTime.TryParse(dobStr, out var tmpDob))
                dob = tmpDob;

            Guid? linkedAccountId = null;
            Account? accountEntity = null;

            if (!string.IsNullOrWhiteSpace(phone))
            {
                accountEntity = await _unitOfWork.Accounts.GetByPhoneAsync(phone);
                if (accountEntity == null)
                {
                    accountEntity = new Account
                    {
                        Id = Guid.NewGuid(),
                        Phone = phone,
                        Email = email,
                        Password = BCrypt.Net.BCrypt.HashPassword("12345678"),
                        RoleName = RoleName.ROLE_CUSTOMER,
                        Stattus = AccountStatus.ACTIVE,
                    };
                    await _unitOfWork.Accounts.CreateAsync(accountEntity);
                }
            }
            else if (accountId.HasValue && accountId.Value != Guid.Empty)
            {
                accountEntity = await _unitOfWork.Accounts.GetByIdAsync(accountId.Value);
            }

            if (accountEntity != null)
                linkedAccountId = accountEntity.Id;

            Customer? customerEntity = null;
            if (linkedAccountId.HasValue)
            {
                customerEntity = (await _unitOfWork.Customers.FindAllAsync())
                    .SingleOrDefault(c => c.AccountId == linkedAccountId.Value);
            }

            if (customerEntity == null)
            {
                customerEntity = new Customer
                {
                    Id = Guid.NewGuid(),
                    AccountId = linkedAccountId,
                    CitizenId = citizenId,
                    FirstName = firstName,
                    LastName = lastName,
                    Address = address,
                    CustomerCode = customerCode,
                    AvatarUrl = avatarUrl,
                    Gender = gender,
                    DateOfBirth = dob,
                };
                await _unitOfWork.Customers.CreateAsync(customerEntity);
            }
            else
            {
                // luôn override bằng data mới nhất từ OEM
                customerEntity.CitizenId = citizenId;
                customerEntity.FirstName = firstName;
                customerEntity.LastName = lastName;
                customerEntity.Address = address;
                customerEntity.CustomerCode = customerCode;
                customerEntity.AvatarUrl = avatarUrl;
                customerEntity.Gender = gender;
                customerEntity.DateOfBirth = dob;

                await _unitOfWork.Customers.UpdateAsync(customerEntity);
            }
            // 1. đọc is_primary từ OEM (nếu có)
            var isPrimaryStr = GetStr(vData, "is_primary");
            bool isPrimaryFromOem = !string.IsNullOrWhiteSpace(isPrimaryStr)
                && bool.TryParse(isPrimaryStr, out var isPrimaryParsed)
                && isPrimaryParsed;

            // 2. kiểm tra số xe hiện có của customer
            var existedVehicles = await _unitOfWork.Vehicles.GetByCustomerIdAsync(customerEntity.Id);
            var existedList = existedVehicles.ToList();
            bool isPrimary =
                isPrimaryFromOem
                || !existedList.Any()
                || !existedList.Any(v => v.IsPrimary);

            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                CustomerId = customerEntity.Id,
                ModelId = modelEntity.Id,
                Image = GetStr(vData, "image") ?? "",
                Color = GetStr(vData, "color") ?? "",
                ChassisNumber = GetStr(vData, "chassis_number") ?? "",
                EngineNumber = GetStr(vData, "engine_number") ?? "",
                Status = StatusEnum.ACTIVE,
                ManufactureDate = TryParseDate(vData, "manufacture_date") ?? DateTime.UtcNow,
                PurchaseDate = TryParseDate(vData, "purchase_date") ?? DateTime.UtcNow,
                WarrantyExpiry = TryParseDate(vData, "warranty_expiry") ?? DateTime.UtcNow,
                IsPrimary = isPrimary,
            };
            await _unitOfWork.Vehicles.CreateAsync(vehicle);
            var vehiclePartItemsData = await _firebase.GetVehiclePartItemsByVehicleIdAsync(vehicleDocIdNew);
            foreach (var vpiData in vehiclePartItemsData)
            {
                var partItemIdStr = GetStr(vpiData, "partItemId");
                if (string.IsNullOrWhiteSpace(partItemIdStr)
                    || !Guid.TryParse(partItemIdStr, out var partItemGuid))
                    continue;

                var createdAt = TryParseDate(vpiData, "createdAt") ?? DateTime.UtcNow;
                var updatedAt = TryParseDate(vpiData, "updatedAt") ?? createdAt;
                var installDate = TryParseDate(vpiData, "installDate");

                var vpiEntity = new VehiclePartItem
                {
                    Id = Guid.NewGuid(),
                    VehicleId = vehicle.Id,
                    PartItemId = partItemGuid,
                    CreatedAt = createdAt,
                    UpdatedAt = updatedAt,
                    InstallDate = installDate ?? createdAt,
                };

                await _unitOfWork.VehiclePartItems.CreateAsync(vpiEntity);
            }

            var vehicleStages = await SyncVehicleStagesFromOemAsync(vehicle.Id, vehicleDocIdNew);
            await _unitOfWork.SaveAsync();

            return (customerEntity, vehicle, vehicleStages);
        }

        public async Task<FirstVisitVehicleInfoResponse> EnsureVehicleFromChassisAsync(string chassisNumber)
        {
            var httpUser = _httpContextAccessor.HttpContext?.User;
            var accountClaim =
                httpUser?.FindFirst(ClaimTypes.NameIdentifier)
                ?? httpUser?.FindFirst("sub")
                ?? httpUser?.FindFirst("accountId");

            Guid? accountId = null;
            if (accountClaim != null && Guid.TryParse(accountClaim.Value, out var accId))
                accountId = accId;

            var (customer, vehicle, vehicleStages) =
                await EnsureVehicleFromVinAsync(chassisNumber, accountId);

            var vpis = await _unitOfWork.VehiclePartItems.GetListByVehicleIdAsync(vehicle.Id);

            return new FirstVisitVehicleInfoResponse
            {
                Customer = _mapper.Map<CustomerResponse>(customer),
                Vehicle = _mapper.Map<VehicleResponse>(vehicle),
                VehicleStages = _mapper.Map<List<VehicleStageShortResponse>>(vehicleStages),
                VehiclePartItems = _mapper.Map<List<VehiclePartItemResponse>>(vpis),
            };
        }

    }
}
