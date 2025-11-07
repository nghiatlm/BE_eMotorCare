using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.AppointmentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly INotifierAppointmentService _notifierAppointment;

        public AppointmentsController(
            IAppointmentService appointmentService,
            INotifierAppointmentService notifierAppointment
        )
        {
            _appointmentService = appointmentService;
            _notifierAppointment = notifierAppointment;
        }

        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] Guid serviceCenterId,
            [FromQuery] DateTime date
        )
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(serviceCenterId, date);
            return Ok(
                ApiResponse<IReadOnlyList<string>>.SuccessResponse(
                    slots,
                    "Lấy khung giờ khả dụng thành công"
                )
            );
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_STAFF,ROLE_MANAGER,ROLE_TECHNICIAN,ROLE_CUSTOMER")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] AppointmentStatus? status,
            [FromQuery] Guid? serviceCenterId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _appointmentService.GetPagedAsync(
                search,
                status,
                serviceCenterId,
                fromDate,
                toDate,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<AppointmentResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách lịch hẹn thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_STAFF,ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _appointmentService.GetByIdAsync(id);
            return Ok(
                ApiResponse<AppointmentResponse>.SuccessResponse(item, "Lấy lịch hẹn thành công")
            );
        }

        [HttpGet("technician/{technicianId}")]
        [Authorize(Roles = "ROLE_TECHNICIAN,ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetByTechnician(Guid technicianId)
        {
            var data = await _appointmentService.GetByTechnicianIdAsync(technicianId);
            return Ok(
                ApiResponse<List<AppointmentResponse>>.SuccessResponse(
                    data,
                    "Lấy cuộc hẹn của technician thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_STAFF,ROLE_MANAGER,ROLE_CUSTOMER")]
        public async Task<IActionResult> Create([FromBody] AppointmentRequest request)
        {
            var id = await _appointmentService.CreateAsync(request);
            await _notifierAppointment.NotifyCreateAsync("Appointment", new { Id = id });
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo lịch hẹn thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_STAFF,ROLE_MANAGER,ROLE_CUSTOMER")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentRequest request)
        {
            await _appointmentService.UpdateAsync(id, request);
            await _notifierAppointment.NotifyUpdateAsync("Appointment", new { Id = id });
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật lịch hẹn thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _appointmentService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá lịch hẹn thành công"));
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> Approve(Guid id, ApproveAppointmentRequest req)
        {
            await _appointmentService.ApproveAsync(id, req.StaffId, req.CheckinQRCode);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Duyệt lịch hẹn thành công"));
        }

        [HttpGet("{id}/getcode")]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> GetCheckinCode(Guid id)
        {
            var code = await _appointmentService.GetCheckinCodeAsync(id);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { code }, "Lấy mã check-in thành công")
            );
        }

        [HttpPost("checkin/by-code")]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> CheckInByCode([FromBody] CheckInRequest req)
        {
            await _appointmentService.CheckInByCodeAsync(req.Code);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Check-in thành công"));
        }
    }
}
