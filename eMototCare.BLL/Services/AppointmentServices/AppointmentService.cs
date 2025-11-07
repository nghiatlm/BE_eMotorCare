using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
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
                    fromDate,
                    toDate,
                    page,
                    pageSize
                );

                var rows = _mapper.Map<List<AppointmentResponse>>(items);
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
                var dow = (eMotoCare.BO.Enums.DayOfWeeks)req.AppointmentDate.DayOfWeek;

                // 1) Có cấu hình slot không?
                var slotCfg = (await _unitOfWork.ServiceCenterSlot.FindAllAsync()).FirstOrDefault(
                    s =>
                        s.ServiceCenterId == req.ServiceCenterId
                        && s.IsActive
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

        public async Task UpdateAsync(Guid id, AppointmentRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Appointments.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

                var changingCore =
                    entity.ServiceCenterId != req.ServiceCenterId
                    || entity.AppointmentDate.Date != req.AppointmentDate.Date
                    || entity.SlotTime != req.SlotTime; // CHANGE

                if (changingCore)
                {
                    var dateOnly = DateOnly.FromDateTime(req.AppointmentDate.Date);
                    var dow = (eMotoCare.BO.Enums.DayOfWeeks)req.AppointmentDate.DayOfWeek;

                    var slotCfg = (
                        await _unitOfWork.ServiceCenterSlot.FindAllAsync()
                    ).FirstOrDefault(s =>
                        s.ServiceCenterId == req.ServiceCenterId
                        && s.IsActive
                        && (s.Date == dateOnly || (s.Date == default && s.DayOfWeek == dow))
                        && s.SlotTime == req.SlotTime
                    );
                    if (slotCfg is null)
                        throw new AppException(
                            "Ngày này không có khung giờ đó.",
                            HttpStatusCode.Conflict
                        );

                    // Nếu đổi sang slot mới, phải check capacity của slot đó ở ngày mới
                    var booked = await _unitOfWork.ServiceCenterSlot.CountBookingsAsync(
                        req.ServiceCenterId,
                        dateOnly,
                        req.SlotTime
                    );

                    // trừ chính record hiện tại nếu nó đang ở cùng ngày+slot
                    if (
                        !(
                            entity.ServiceCenterId == req.ServiceCenterId
                            && DateOnly.FromDateTime(entity.AppointmentDate.Date) == dateOnly
                            && entity.SlotTime == req.SlotTime
                        )
                    )
                    {
                        if (booked >= slotCfg.Capacity)
                            throw new AppException(
                                "Khung giờ này đã đầy.",
                                HttpStatusCode.Conflict
                            );
                    }

                    entity.ServiceCenterId = req.ServiceCenterId;
                    entity.AppointmentDate = req.AppointmentDate;
                    entity.SlotTime = req.SlotTime; // CHANGE
                }

                // map các field khác
                entity.CustomerId = req.CustomerId;
                entity.VehicleStageId = req.VehicleStageId;
                entity.EstimatedCost = req.EstimatedCost;
                entity.ActualCost = req.ActualCost;
                entity.Status = req.Status;
                entity.Type = req.Type;

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

        public async Task<string> GetCheckinCodeAsync(Guid id)
        {
            var entity =
                await _unitOfWork.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);
            return entity.Code;
        }

        public async Task ApproveAsync(Guid id, Guid staffId, string checkinQRCode)
        {
            if (string.IsNullOrWhiteSpace(checkinQRCode))
                throw new AppException("Mã check-in không hợp lệ", HttpStatusCode.BadRequest);

            var entity =
                await _unitOfWork.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            entity.Status = AppointmentStatus.APPROVED;
            entity.ApproveById = staffId;
            entity.CheckinQRCode = checkinQRCode;

            await _unitOfWork.Appointments.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateStatusAsync(Guid id, AppointmentStatus status)
        {
            var entity =
                await _unitOfWork.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            entity.Status = status;
            await _unitOfWork.Appointments.UpdateAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public async Task CheckInByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new AppException("Mã check-in không hợp lệ", HttpStatusCode.BadRequest);

            var appt =
                await _unitOfWork.Appointments.GetByCodeAsync(code.Trim())
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            switch (appt.Status)
            {
                case AppointmentStatus.PENDING:
                case AppointmentStatus.APPROVED:
                    appt.Status = AppointmentStatus.CHECKED_IN;
                    await _unitOfWork.Appointments.UpdateAsync(appt);
                    await _unitOfWork.SaveAsync();
                    break;

                case AppointmentStatus.CHECKED_IN:
                    break;

                default:
                    throw new AppException(
                        "Trạng thái lịch hẹn không cho phép check-in",
                        HttpStatusCode.Conflict
                    );
            }
        }

        public async Task AssignTechnicianAsync(
            Guid appointmentId,
            Guid technicianId,
            Guid approveById
        )
        {
            var appt =
                await _unitOfWork.Appointments.GetByIdAsync(appointmentId)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);
            appt.ApproveById = approveById;
            appt.Status = AppointmentStatus.APPROVED;
            await _unitOfWork.Appointments.UpdateAsync(appt);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<AppointmentResponse>> GetByTechnicianIdAsync(Guid technicianId)
        {
            var appointments = await _unitOfWork.Appointments.GetByTechnicianIdAsync(technicianId);
            return _mapper.Map<List<AppointmentResponse>>(appointments);
        }
    }
}
