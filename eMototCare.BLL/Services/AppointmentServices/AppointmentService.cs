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
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.AppointmentServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AppointmentService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
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
                if (req.AppointmentDate.Date < now)
                {
                    throw new AppException("Ngày đặt phải từ hôm nay trở đi.");
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
                // 1) Có cấu hình slot không?
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

                // 2) Đếm appointment đã đặt trong (SC, Date, SlotTime)
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

                // 3) Sinh code
                string code;
                int guard = 0;
                do
                {
                    code = $"APPT-{req.AppointmentDate:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                    guard++;
                } while (await _unitOfWork.Appointments.ExistsCodeAsync(code) && guard < 5);

                // 4) Tạo Appointment (không gán ServiceCenterSlotId)
                var entity = _mapper.Map<Appointment>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = code;

                await _unitOfWork.Appointments.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

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
                entity.EstimatedCost = req.EstimatedCost;
                entity.ActualCost = req.ActualCost;
                if (req.Note != null)
                    entity.Note = req.Note;

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
        ) => _unitOfWork.Appointments.GetAvailableSlotsAsync(serviceCenterId, date);

        public async Task UpdateStatusAsync(Guid id, AppointmentStatus status)
        {
            var entity =
                await _unitOfWork.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            entity.Status = status;
            await _unitOfWork.Appointments.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<AppointmentResponse>> GetByTechnicianIdAsync(Guid technicianId)
        {
            var appointments = await _unitOfWork.Appointments.GetByTechnicianIdAsync(technicianId);
            return _mapper.Map<List<AppointmentResponse>>(appointments);
        }

        public async Task<List<MissingPartResponse>> GetMissingPartsAsync(Guid appointmentId)
        {
            try
            {
                // 1) Lấy appointment (để biết ServiceCenter + thông tin hiển thị)
                var appt =
                    await _unitOfWork.Appointments.GetByIdAsync(appointmentId)
                    ?? throw new AppException(
                        "Không tìm thấy Appointment",
                        HttpStatusCode.NotFound
                    );

                // 2) Lấy EVCheck (kèm EVCheckDetails). Repo nên include MSD + PartItem(+Part) nếu có thể
                var evCheck = await _unitOfWork.EVChecks.GetByAppointmentIdAsync(appointmentId);
                if (
                    evCheck == null
                    || evCheck.EVCheckDetails == null
                    || !evCheck.EVCheckDetails.Any()
                )
                    return new List<MissingPartResponse>();

                // 3) Tồn kho theo Service Center
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
                    if (d.ReplacePartId.HasValue)
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

                // 5) Build kết quả cho các Part còn thiếu
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

                var note = string.Join(
                    "; ",
                    evCheck
                        .EVCheckDetails.Select(x => x.Result)
                        .Where(r => !string.IsNullOrWhiteSpace(r))
                        .Distinct()
                );

                return new List<MissingPartResponse>
                {
                    new MissingPartResponse
                    {
                        AppointmentId = appt.Id,
                        ServiceCenterId = appt.ServiceCenterId,
                        ServiceCenterName = appt.ServiceCenter?.Name ?? "",
                        TaskExecutorId = evCheck.TaskExecutorId,
                        RequestedAt = evCheck.CheckDate,
                        CreatedById = evCheck.TaskExecutorId,
                        Status = "DRAFT",
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
    }
}
