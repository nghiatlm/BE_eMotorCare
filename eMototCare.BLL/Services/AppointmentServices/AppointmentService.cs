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
                if (req.VehicleStageId.HasValue)
                {
                    var stage = await _unitOfWork.VehicleStages.GetByIdAsync(
                        req.VehicleStageId.Value
                    );
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
                            if (entity.VehicleStageId.HasValue)
                            {
                                var stage = await _unitOfWork.VehicleStages.GetByIdAsync(
                                    entity.VehicleStageId.Value
                                );
                                if (stage != null && stage.Status != VehicleStageStatus.COMPLETED)
                                {
                                    stage.Status = VehicleStageStatus.COMPLETED;
                                    stage.DateOfImplementation = DateTime.UtcNow;

                                    await _unitOfWork.VehicleStages.UpdateAsync(stage);
                                    _logger.LogInformation(
                                        "VehicleStage {StageId} -> COMPLETED (từ Appointment {Id})",
                                        stage.Id,
                                        entity.Id
                                    );
                                }
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

        public async Task<List<AppointmentResponse>> GetByTechnicianIdAsync(Guid technicianId)
        {
            if (technicianId == Guid.Empty)
                throw new AppException("TechnicianId không hợp lệ", HttpStatusCode.BadRequest);
            var appointments = await _unitOfWork.Appointments.GetByTechnicianIdAsync(technicianId);
            return _mapper.Map<List<AppointmentResponse>>(appointments);
        }

        public async Task<List<MissingPartResponse>> GetMissingPartsAsync(
            Guid? appointmentId,
            string? sortBy,
            bool sortDesc,
            int page,
            int pageSize
        )
        {
            try
            {
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (appointmentId.HasValue && appointmentId.Value == Guid.Empty)
                    throw new AppException("AppointmentId không hợp lệ", HttpStatusCode.BadRequest);
                if (!appointmentId.HasValue)
                {
                    var result = new List<MissingPartResponse>();

                    var allAppointments = await _unitOfWork.Appointments.FindAllAsync();

                    foreach (var apptItem in allAppointments)
                    {
                        var evCheckItem = await _unitOfWork.EVChecks.GetByAppointmentIdAsync(
                            apptItem.Id
                        );
                        if (
                            evCheckItem == null
                            || evCheckItem.EVCheckDetails == null
                            || !evCheckItem.EVCheckDetails.Any()
                        )
                            continue;

                        var single = await GetMissingPartsAsync(
                            apptItem.Id,
                            sortBy,
                            sortDesc,
                            1,
                            int.MaxValue
                        );
                        if (single != null && single.Any())
                            result.AddRange(single);
                    }

                    return result;
                }

                var appt =
                    await _unitOfWork.Appointments.GetByIdAsync(appointmentId.Value)
                    ?? throw new AppException(
                        "Không tìm thấy Appointment",
                        HttpStatusCode.NotFound
                    );

                var evCheck = await _unitOfWork.EVChecks.GetByAppointmentIdAsync(
                    appointmentId.Value
                );
                if (
                    evCheck == null
                    || evCheck.EVCheckDetails == null
                    || !evCheck.EVCheckDetails.Any()
                )
                    return new List<MissingPartResponse>();

                var scId = appt.ServiceCenterId;
                var inventoryItems = await _unitOfWork.PartItems.GetByServiceCenterIdAsync(scId);

                var availableByPart = inventoryItems
                    .Where(pi => pi.Status == PartItemStatus.ACTIVE && pi.Quantity > 0)
                    .GroupBy(pi => pi.PartId)
                    .ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            AvailableQty = g.Sum(x => x.Quantity),
                            Part = g.FirstOrDefault()?.Part,
                        }
                    );

                var needed = new Dictionary<Guid, int>();
                var neededDetails = new Dictionary<Guid, List<EVCheckDetail>>();

                foreach (var d in evCheck.EVCheckDetails)
                {
                    if (d.Status == EVCheckDetailStatus.CANCELED)
                        continue;
                    if (d.ProposedReplacePartId.HasValue)
                        continue;

                    Guid? requiredPartId = null;

                    if (d.MaintenanceStageDetailId.HasValue)
                    {
                        var msd =
                            d.MaintenanceStageDetail
                            ?? await _unitOfWork.MaintenanceStageDetails.GetByIdAsync(
                                d.MaintenanceStageDetailId.Value
                            );
                        requiredPartId = msd?.PartId;
                    }

                    if (!requiredPartId.HasValue && d.PartItemId != Guid.Empty)
                    {
                        var pi =
                            d.PartItem ?? await _unitOfWork.PartItems.GetByIdAsync(d.PartItemId);
                        requiredPartId = pi?.PartId;
                    }

                    if (!requiredPartId.HasValue)
                        continue;

                    var pid = requiredPartId.Value;
                    if (!needed.ContainsKey(pid))
                        needed[pid] = 0;
                    needed[pid] += 1;

                    if (!neededDetails.ContainsKey(pid))
                        neededDetails[pid] = new List<EVCheckDetail>();
                    neededDetails[pid].Add(d);
                }

                // 5) Build kết quả cho các Part còn thiếu (details)
                var details = new List<MissingPartDetailResponse>();
                int index = 1;
                foreach (var kv in needed)
                {
                    var partId = kv.Key;
                    var neededQty = kv.Value;

                    var availInfo = availableByPart.TryGetValue(partId, out var ai) ? ai : null;
                    var availableQty = availInfo?.AvailableQty ?? 0;
                    var missingQty = Math.Max(neededQty - availableQty, 0);

                    // chỉ trả những cái còn thiếu
                    if (missingQty <= 0)
                        continue;

                    var part = availInfo?.Part ?? await _unitOfWork.Parts.GetByIdAsync(partId);

                    var suggest = "CN khác (có thể điều chuyển)";
                    if (availableQty > 0)
                        suggest = $"{appt.ServiceCenter?.Name} (Kho hiện có {availableQty})";

                    details.Add(
                        new MissingPartDetailResponse
                        {
                            Index = index++,
                            Image = part?.Image,
                            Code = part?.Code ?? "",
                            Name = part?.Name ?? "",
                            RequestedQty = neededQty,
                            SuggestCenter = suggest,
                            StockStatus = availableQty > 0 ? "Có thể điều chuyển" : "Hết hàng",
                        }
                    );
                }

                // Tổng số lượng để FE dùng nếu cần
                var totalNeeded = needed.Values.Sum();

                var totalAvailable = availableByPart
                    .Where(kv => needed.ContainsKey(kv.Key))
                    .Sum(kv => kv.Value.AvailableQty);

                var totalMissing = Math.Max(totalNeeded - totalAvailable, 0);

                var note = string.Join(
                    "; ",
                    evCheck
                        .EVCheckDetails.Select(x => x.Result)
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Distinct()
                );

                var requestCode = $"REQ-{appt.AppointmentDate:yyyy}-{appt.Code}";
                string createdByName = string.Empty;
                var staff = await _unitOfWork.Staffs.GetByIdAsync(evCheck.TaskExecutorId);
                if (staff != null)
                    createdByName = $"{staff.FirstName} {staff.LastName}".Trim();

                return new List<MissingPartResponse>
                {
                    new MissingPartResponse
                    {
                        AppointmentId = appt.Id,

                        ServiceCenterId = appt.ServiceCenterId,
                        ServiceCenterName = appt.ServiceCenter?.Name ?? "",

                        RequestCode = requestCode,
                        RequestedAt = evCheck.CheckDate,

                        CreatedById = evCheck.TaskExecutorId,
                        CreatedByName = createdByName,

                        //Status = "DRAFT",
                        Note = string.IsNullOrWhiteSpace(note) ? null : note,

                        Details = details,
                    },
                };
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "GetMissingPartsAsync failed for {AppointmentId}: {Message}",
                    appointmentId,
                    ex.Message
                );
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        private async Task<(
            Customer Customer,
            Vehicle Vehicle,
            VehicleStage? VehicleStage
        )> EnsureVehicleFromVinAsync(string chassisNumber, Guid? accountId)
        {
            if (string.IsNullOrWhiteSpace(chassisNumber))
                throw new AppException(
                    "ChassisNumber không được để trống",
                    HttpStatusCode.BadRequest
                );
            var localVehicle = await _unitOfWork.Vehicles.GetByChassisNumberAsync(chassisNumber);
            if (localVehicle != null)
            {
                var stages = await _unitOfWork.VehicleStages.GetByVehicleIdAsync(localVehicle.Id);
                var upcoming =
                    stages
                        .Where(s => s.Status == VehicleStageStatus.UPCOMING)
                        .OrderBy(s => s.DateOfImplementation)
                        .FirstOrDefault()
                    ?? stages.OrderByDescending(s => s.DateOfImplementation).FirstOrDefault();

                var customer =
                    localVehicle.Customer
                    ?? (
                        localVehicle.CustomerId.HasValue
                            ? await _unitOfWork.Customers.GetByIdAsync(
                                localVehicle.CustomerId.Value
                            )
                            : throw new AppException(
                                "Customer ID is null",
                                HttpStatusCode.BadRequest
                            )
                    );

                return (customer, localVehicle, upcoming);
            }

            if (!_firebase.IsFirestoreConfigured())
                throw new AppException(
                    "Không thể kết nối OEM (Firestore)",
                    HttpStatusCode.InternalServerError
                );

            var fireVehicle = await _firebase.GetVehicleByChassisNumberAsync(chassisNumber);
            if (fireVehicle is null)
                throw new AppException(
                    "Không tìm thấy xe trong hệ thống OEM",
                    HttpStatusCode.NotFound
                );

            var vData = fireVehicle.Value.Data;
            var vehicleDocId = fireVehicle.Value.Id;

            Model? modelEntity = null;
            string? oemModelId = null;

            if (vData.TryGetValue("modelId", out var mObj) && mObj != null)
                oemModelId = mObj.ToString();
            else if (vData.TryGetValue("model_id", out var m2Obj) && m2Obj != null)
                oemModelId = m2Obj.ToString();

            if (!string.IsNullOrWhiteSpace(oemModelId))
            {
                var modelData = await _firebase.GetModelByIdAsync(oemModelId);

                if (modelData != null)
                {
                    string modelName =
                        modelData.TryGetValue("name", out var nameObj) && nameObj != null
                            ? nameObj.ToString()!
                        : modelData.TryGetValue("Name", out var nameObj2) && nameObj2 != null
                            ? nameObj2.ToString()!
                        : string.Empty;

                    string modelCode =
                        modelData.TryGetValue("code", out var codeObj) && codeObj != null
                            ? codeObj.ToString()!
                        : modelData.TryGetValue("Code", out var codeObj2) && codeObj2 != null
                            ? codeObj2.ToString()!
                        : string.Empty;

                    string? statusStr =
                        modelData.TryGetValue("status", out var stObj) && stObj != null
                            ? stObj.ToString()
                        : modelData.TryGetValue("Status", out var stObj2) && stObj2 != null
                            ? stObj2.ToString()
                        : null;

                    var status = StatusEnum.ACTIVE;
                    if (
                        !string.IsNullOrWhiteSpace(statusStr)
                        && Enum.TryParse<StatusEnum>(statusStr, true, out var parsedStatus)
                    )
                    {
                        status = parsedStatus;
                    }

                    var allModels = await _unitOfWork.Models.FindAllAsync();
                    modelEntity = allModels.FirstOrDefault(m =>
                        (!string.IsNullOrEmpty(modelCode) && m.Code == modelCode)
                        || (!string.IsNullOrEmpty(modelName) && m.Name == modelName)
                    );

                    if (modelEntity == null)
                    {
                        modelEntity = new Model
                        {
                            Id = Guid.NewGuid(),
                            Name = modelName,
                            Code = modelCode,
                            Status = (Status)status,
                        };

                        await _unitOfWork.Models.CreateAsync(modelEntity);
                    }
                }
            }

            if (modelEntity == null)
            {
                throw new AppException(
                    "Không tìm thấy hoặc sync được Model tương ứng với xe OEM. "
                        + "Vui lòng kiểm tra lại dữ liệu model trên OEM/DB.",
                    HttpStatusCode.BadRequest
                );
            }

            if (!vData.TryGetValue("customerId", out var custIdObj) || custIdObj == null)
                throw new AppException(
                    "Xe trong OEM không có thông tin khách hàng",
                    HttpStatusCode.BadRequest
                );

            var custData = await _firebase.GetCustomerByIdAsync(custIdObj.ToString()!);
            if (custData == null)
                throw new AppException(
                    "Không sync được khách hàng từ OEM",
                    HttpStatusCode.BadRequest
                );

            var citizenId = custData.TryGetValue("citizenId", out var ciObj)
                ? ciObj?.ToString() ?? ""
                : "";
            var firstName = custData.TryGetValue("firstName", out var fnObj)
                ? fnObj?.ToString() ?? ""
                : "";
            var lastName = custData.TryGetValue("lastName", out var lnObj)
                ? lnObj?.ToString() ?? ""
                : "";
            var address = custData.TryGetValue("address", out var adObj)
                ? adObj?.ToString() ?? ""
                : "";
            var customerCode = custData.TryGetValue("customerCode", out var ccObj)
                ? ccObj?.ToString() ?? ""
                : "";
            var avatarUrl = custData.TryGetValue("avatarUrl", out var avObj)
                ? avObj?.ToString() ?? ""
                : "";

            var phone = custData.TryGetValue("phone", out var phObj) ? phObj?.ToString() ?? "" : "";
            var emailRaw = custData.TryGetValue("email", out var emObj) ? emObj?.ToString() : null;
            var email = string.IsNullOrWhiteSpace(emailRaw)
                ? null
                : emailRaw.Trim().ToLowerInvariant();

            var genderStr = custData.TryGetValue("gender", out var gObj) ? gObj?.ToString() : null;
            var dobStr = custData.TryGetValue("dateOfBirth", out var dobObj)
                ? dobObj?.ToString()
                : null;

            GenderEnum? gender = null;
            if (
                !string.IsNullOrWhiteSpace(genderStr)
                && Enum.TryParse<GenderEnum>(genderStr, true, out var g)
            )
                gender = g;

            DateTime? dob = null;
            if (!string.IsNullOrWhiteSpace(dobStr) && DateTime.TryParse(dobStr, out var tmpDob))
                dob = tmpDob;

            Guid? linkedAccountId = null;
            Account? accountEntity = null;

            if (accountId.HasValue && accountId.Value != Guid.Empty)
            {
                accountEntity = await _unitOfWork.Accounts.GetByIdAsync(accountId.Value);
            }

            if (accountEntity == null && !string.IsNullOrWhiteSpace(phone))
            {
                accountEntity = await _unitOfWork.Accounts.GetByPhoneAsync(phone);
            }

            if (accountEntity == null && !string.IsNullOrWhiteSpace(phone))
            {
                var existingByPhone = await _unitOfWork.Accounts.GetByPhoneAsync(phone);
                if (existingByPhone != null)
                {
                    accountEntity = existingByPhone;
                }
                else
                {
                    accountEntity = new Account
                    {
                        Id = Guid.NewGuid(),
                        Phone = phone,
                        Email = email,
                        Password = BCrypt.Net.BCrypt.HashPassword("123456"),
                        RoleName = RoleName.ROLE_CUSTOMER,
                        Stattus = AccountStatus.ACTIVE,
                    };

                    await _unitOfWork.Accounts.CreateAsync(accountEntity);
                }
            }

            if (accountEntity != null)
                linkedAccountId = accountEntity.Id;

            var customerEntity = new Customer
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

            var vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                CustomerId = customerEntity.Id,
                ModelId = modelEntity.Id,

                Image = vData.TryGetValue("image", out var imgObj) ? imgObj?.ToString() ?? "" : "",
                Color = vData.TryGetValue("color", out var colorObj)
                    ? colorObj?.ToString() ?? ""
                    : "",
                ChassisNumber = vData.TryGetValue("chassis_number", out var chObj)
                    ? chObj?.ToString() ?? ""
                    : "",
                EngineNumber = vData.TryGetValue("engine_number", out var enObj)
                    ? enObj?.ToString() ?? ""
                    : "",
                Status = StatusEnum.ACTIVE,
                ManufactureDate = TryParseDate(vData, "manufacture_date") ?? DateTime.UtcNow,
                PurchaseDate = TryParseDate(vData, "purchase_date") ?? DateTime.UtcNow,
                WarrantyExpiry = TryParseDate(vData, "warranty_expiry") ?? DateTime.UtcNow,
            };
            await _unitOfWork.Vehicles.CreateAsync(vehicle);

            var stageDataList = await _firebase.GetVehicleStagesByVehicleIdAsync(vehicleDocId);
            var vehicleStages = new List<VehicleStage>();

            foreach (var sd in stageDataList)
            {
                Guid? maintenanceStageId = null;

                if (sd.TryGetValue("maintenancestageId", out var msObj) && msObj != null)
                {
                    var msIdStr = msObj.ToString();

                    if (Guid.TryParse(msIdStr, out var msIdParsed))
                    {
                        var msEntity = await _unitOfWork.MaintenanceStages.GetByIdAsync(msIdParsed);

                        if (msEntity == null)
                        {
                            var msData = await _firebase.GetMaintenanceStageByIdAsync(msIdStr);
                            if (msData != null)
                            {
                                Guid? maintenancePlanId = null;
                                if (
                                    msData.TryGetValue("maintenancePlanId", out var mpObj)
                                    && mpObj != null
                                    && Guid.TryParse(mpObj.ToString(), out var mpGuid)
                                )
                                {
                                    var mpEntity = await _unitOfWork.MaintenancePlans.GetByIdAsync(
                                        mpGuid
                                    );
                                    if (mpEntity == null)
                                    {
                                        var mpCode = msData.TryGetValue("planCode", out var pcObj)
                                            ? pcObj?.ToString() ?? ""
                                            : $"OEM-{mpGuid.ToString()[..8]}";

                                        var mpName = msData.TryGetValue("planName", out var pnObj)
                                            ? pnObj?.ToString() ?? "OEM Maintenance Plan"
                                            : "OEM Maintenance Plan";

                                        var mpDesc = msData.TryGetValue(
                                            "description",
                                            out var mpDescObj
                                        )
                                            ? mpDescObj?.ToString() ?? ""
                                            : "";

                                        var unitStr = msData.TryGetValue("unit", out var uObj)
                                            ? uObj?.ToString()
                                            : null;
                                        var effectiveDateStr = msData.TryGetValue(
                                            "effectiveDate",
                                            out var edObj
                                        )
                                            ? edObj?.ToString()
                                            : null;

                                        MaintenanceUnit unit = MaintenanceUnit.KILOMETER;
                                        if (
                                            !string.IsNullOrWhiteSpace(unitStr)
                                            && Enum.TryParse<MaintenanceUnit>(
                                                unitStr,
                                                true,
                                                out var parsedUnit
                                            )
                                        )
                                        {
                                            unit = parsedUnit;
                                        }

                                        DateTime effectiveDate = DateTime.UtcNow;
                                        if (
                                            !string.IsNullOrWhiteSpace(effectiveDateStr)
                                            && DateTime.TryParse(effectiveDateStr, out var edParsed)
                                        )
                                        {
                                            effectiveDate = edParsed;
                                        }

                                        mpEntity = new MaintenancePlan
                                        {
                                            Id = mpGuid,
                                            Code = mpCode,
                                            Name = mpName,
                                            Description = mpDesc,
                                            Status = Status.ACTIVE,
                                            Unit = new[] { unit },
                                            EffectiveDate = effectiveDate,
                                            TotalStages = 0,
                                        };

                                        await _unitOfWork.MaintenancePlans.CreateAsync(mpEntity);
                                    }

                                    maintenancePlanId = mpEntity.Id;
                                }

                                if (!maintenancePlanId.HasValue)
                                {
                                    continue;
                                }

                                var name = msData.TryGetValue("name", out var nObj)
                                    ? nObj?.ToString() ?? ""
                                    : "";
                                var description = msData.TryGetValue("description", out var dObj)
                                    ? dObj?.ToString() ?? ""
                                    : "";
                                var mileage = msData.TryGetValue("mileage", out var mileageObj)
                                    ? mileageObj?.ToString() ?? ""
                                    : "";
                                var msStatusStr = msData.TryGetValue("status", out var sObj)
                                    ? sObj?.ToString()
                                    : null;

                                var msStatus = StatusEnum.ACTIVE;
                                if (
                                    !string.IsNullOrWhiteSpace(msStatusStr)
                                    && Enum.TryParse<StatusEnum>(
                                        msStatusStr,
                                        true,
                                        out var stParsed
                                    )
                                )
                                {
                                    msStatus = stParsed;
                                }

                                msEntity = new MaintenanceStage
                                {
                                    Id = msIdParsed,
                                    MaintenancePlanId = maintenancePlanId.Value,
                                    Name = name,
                                    Description = description,
                                    Mileage = Enum.TryParse<Mileage>(mileage, out var parsedMileage)
                                        ? parsedMileage
                                        : throw new AppException(
                                            "Invalid mileage value.",
                                            HttpStatusCode.BadRequest
                                        ),
                                    Status = (Status)msStatus,
                                };

                                await _unitOfWork.MaintenanceStages.CreateAsync(msEntity);
                            }
                        }

                        if (msEntity != null)
                            maintenanceStageId = msEntity.Id;
                    }
                }

                if (!maintenanceStageId.HasValue)
                    continue;

                var statusStr = sd.TryGetValue("status", out var stObj) ? stObj?.ToString() : null;
                var vsStatus = VehicleStageStatus.UPCOMING;
                if (
                    !string.IsNullOrWhiteSpace(statusStr)
                    && Enum.TryParse<VehicleStageStatus>(statusStr, true, out var sParsed)
                )
                {
                    vsStatus = sParsed;
                }

                var vs = new VehicleStage
                {
                    Id = Guid.NewGuid(),
                    VehicleId = vehicle.Id,
                    MaintenanceStageId = maintenanceStageId.Value,
                    ActualMaintenanceMileage = 0,
                    ActualMaintenanceUnit = MaintenanceUnit.KILOMETER,
                    DateOfImplementation =
                        TryParseDate(sd, "dateOfImplementation") ?? DateTime.UtcNow,
                    Status = vsStatus,
                };

                await _unitOfWork.VehicleStages.CreateAsync(vs);
                vehicleStages.Add(vs);
            }
            VehicleStage? selectedStage =
                vehicleStages
                    .Where(s => s.Status == VehicleStageStatus.UPCOMING)
                    .OrderBy(s => s.DateOfImplementation)
                    .FirstOrDefault()
                ?? vehicleStages.OrderByDescending(s => s.DateOfImplementation).FirstOrDefault();

            await _unitOfWork.SaveAsync();

            return (customerEntity, vehicle, selectedStage);
        }

        private static DateTime? TryParseDate(Dictionary<string, object> dict, string key)
        {
            if (!dict.TryGetValue(key, out var obj) || obj == null)
                return null;
            if (DateTime.TryParse(obj.ToString(), out var dt))
                return dt;
            return null;
        }

        public async Task<FirstVisitVehicleInfoResponse> EnsureVehicleFromChassisAsync(
            string chassisNumber
        )
        {
            var httpUser = _httpContextAccessor.HttpContext?.User;
            var accountClaim =
                httpUser?.FindFirst(ClaimTypes.NameIdentifier)
                ?? httpUser?.FindFirst("sub")
                ?? httpUser?.FindFirst("accountId");

            Guid? accountId = null;
            if (accountClaim != null && Guid.TryParse(accountClaim.Value, out var accId))
                accountId = accId;

            var (customer, vehicle, vehicleStage) = await EnsureVehicleFromVinAsync(
                chassisNumber,
                accountId
            );

            return new FirstVisitVehicleInfoResponse
            {
                Customer = _mapper.Map<CustomerResponse>(customer),
                Vehicle = _mapper.Map<VehicleResponse>(vehicle),
                VehicleStage =
                    vehicleStage != null ? _mapper.Map<VehicleStageResponse>(vehicleStage) : null,
            };
        }
    }
}
