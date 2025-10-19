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
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(
            IUnitOfWork uow,
            IMapper mapper,
            ILogger<AppointmentService> logger
        )
        {
            _uow = uow;
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
                var (items, total) = await _uow.Appointments.GetPagedAsync(
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
                await _uow.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            return _mapper.Map<AppointmentResponse>(appt);
        }

        public async Task<Guid> CreateAsync(AppointmentRequest req)
        {
            try
            {
                if (
                    await _uow.Appointments.ExistsOverlapAsync(
                        req.ServiceCenterId,
                        req.AppointmentDate,
                        req.TimeSlot
                    )
                )
                    throw new AppException("Khung giờ đã được đặt", HttpStatusCode.Conflict);

                string code;
                int guard = 0;
                do
                {
                    code = $"APPT-{req.AppointmentDate:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                    guard++;
                } while (await _uow.Appointments.ExistsCodeAsync(code) && guard < 5);

                var entity = _mapper.Map<Appointment>(req);
                entity.Id = Guid.NewGuid();
                entity.Code = code;

                await _uow.Appointments.CreateAsync(entity);
                await _uow.SaveAsync();

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
                    await _uow.Appointments.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

                if (
                    (
                        entity.ServiceCenterId != req.ServiceCenterId
                        || entity.AppointmentDate.Date != req.AppointmentDate.Date
                        || !string.Equals(
                            entity.TimeSlot,
                            req.TimeSlot,
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    && await _uow.Appointments.ExistsOverlapAsync(
                        req.ServiceCenterId,
                        req.AppointmentDate,
                        req.TimeSlot
                    )
                )
                {
                    throw new AppException("Khung giờ đã được đặt", HttpStatusCode.Conflict);
                }

                _mapper.Map(req, entity);
                await _uow.Appointments.UpdateAsync(entity);
                await _uow.SaveAsync();

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
                    await _uow.Appointments.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

                await _uow.Appointments.DeleteAsync(entity);
                await _uow.SaveAsync();

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
        ) => _uow.Appointments.GetAvailableSlotsAsync(serviceCenterId, date);

        public async Task<string> GetCheckinCodeAsync(Guid id)
        {
            var entity =
                await _uow.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);
            return entity.Code; // có thể encode QR ở frontend từ string này
        }

        public async Task ApproveAsync(Guid id, Guid staffId)
        {
            var entity =
                await _uow.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            entity.Status = AppointmentStatus.APPROVED;
            entity.ApproveById = staffId;

            await _uow.Appointments.UpdateAsync(entity);
            await _uow.SaveAsync();
        }

        public async Task UpdateStatusAsync(Guid id, AppointmentStatus status)
        {
            var entity =
                await _uow.Appointments.GetByIdAsync(id)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            entity.Status = status;
            await _uow.Appointments.UpdateAsync(entity);
            await _uow.SaveAsync();
        }

        public async Task AssignTechnicianAsync(
            Guid appointmentId,
            Guid technicianId,
            Guid approveById
        )
        {
            var appt =
                await _uow.Appointments.GetByIdAsync(appointmentId)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);
            appt.ApproveById = approveById;
            appt.Status = AppointmentStatus.APPROVED;
            await _uow.Appointments.UpdateAsync(appt);
            await _uow.SaveAsync();
        }

        public async Task CheckInAsync(Guid appointmentId)
        {
            var appt =
                await _uow.Appointments.GetByIdAsync(appointmentId)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            appt.Status = AppointmentStatus.IN_SERVICE;
            await _uow.Appointments.UpdateAsync(appt);
            await _uow.SaveAsync();
        }

        public async Task<EVCheckResponse> UpsertEVCheckAsync(
            Guid appointmentId,
            EVCheckUpsertRequest req,
            Guid technicianId
        )
        {
            var appt =
                await _uow.Appointments.GetByIdAsync(appointmentId)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            var ev = await _uow.EVChecks.GetByAppointmentIdAsync(appointmentId);
            if (ev == null)
            {
                ev = _mapper.Map<EVCheck>(req);
                ev.Id = Guid.NewGuid();
                ev.AppointmentId = appointmentId;
                ev.TaskExecutorId = technicianId;
                await _uow.EVChecks.CreateAsync(ev);
            }
            else
            {
                ev.CheckDate = req.CheckDate;
                ev.Odometer = req.Odometer;
                ev.Status = req.Status;
                ev.TotalAmout = req.TotalAmout;
                await _uow.EVChecks.UpdateAsync(ev);

                var olds = ev.EVCheckDetails?.ToList() ?? new();
                if (olds.Count > 0)
                    _uow.RemoveRange(olds);
            }

            ev.EVCheckDetails = new List<EVCheckDetail>();
            foreach (var i in req.Items)
            {
                var item = _mapper.Map<EVCheckDetail>(i);
                item.Id = Guid.NewGuid();
                item.EVCheckId = ev.Id;
                item.Status = Status.ACTIVE;

                var qty = i.Quantity ?? 1m;
                var part = i.PricePart ?? 0m;
                var svc = i.PriceService ?? 0m;
                item.TotalAmount ??= (part + svc) * qty;
                ev.EVCheckDetails.Add(item);
            }

            ev.TotalAmout ??= ev.EVCheckDetails.Sum(x => x.TotalAmount ?? 0m);

            await _uow.SaveAsync();

            return _mapper.Map<EVCheckResponse>(
                await _uow.EVChecks.GetByIdIncludeDetailsAsync(ev.Id)!
            );
        }

        public async Task<EVCheckResponse?> GetEVCheckAsync(Guid appointmentId)
        {
            var ev = await _uow.EVChecks.GetByAppointmentIdAsync(appointmentId);
            return ev is null ? null : _mapper.Map<EVCheckResponse>(ev);
        }

        public async Task ConfirmInspectionAsync(Guid appointmentId, InspectionConfirmRequest req)
        {
            var appt =
                await _uow.Appointments.GetByIdAsync(appointmentId)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);

            if (!req.Accept)
            {
                appt.Status = AppointmentStatus.CANCELED;
            }
            else
            {
                appt.Status = AppointmentStatus.IN_SERVICE; // đồng ý sửa
            }

            await _uow.Appointments.UpdateAsync(appt);
            await _uow.SaveAsync();
        }

        public async Task StartRepairAsync(Guid appointmentId)
        {
            var appt =
                await _uow.Appointments.GetByIdAsync(appointmentId)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);
            appt.Status = AppointmentStatus.IN_SERVICE;
            await _uow.Appointments.UpdateAsync(appt);
            await _uow.SaveAsync();
        }

        public async Task FinishRepairAsync(Guid appointmentId)
        {
            var appt =
                await _uow.Appointments.GetByIdAsync(appointmentId)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);
            appt.Status = AppointmentStatus.APPROVED;
            await _uow.Appointments.UpdateAsync(appt);
            await _uow.SaveAsync();
        }

        public async Task<RepairTicketResponse> GetRepairTicketAsync(Guid appointmentId)
        {
            var appt =
                await _uow.Appointments.GetByIdAsync(appointmentId)
                ?? throw new AppException("Không tìm thấy lịch hẹn", HttpStatusCode.NotFound);
            var ev =
                await _uow.EVChecks.GetByAppointmentIdAsync(appointmentId)
                ?? throw new AppException("Chưa có kết quả kiểm tra", HttpStatusCode.BadRequest);

            var resp = _mapper.Map<EVCheckResponse>(ev);
            var total = Convert.ToDouble(resp.Items.Sum(x => (x.TotalAmount ?? 0m)));

            return new RepairTicketResponse
            {
                AppointmentId = appt.Id,
                AppointmentCode = appt.Code,
                AppointmentDate = appt.AppointmentDate,
                TimeSlot = appt.TimeSlot,
                EVCheck = resp,
                TotalMustPay = total,
            };
        }
    }
}
