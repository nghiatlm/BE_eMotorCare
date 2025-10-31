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
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet("available-slots")]
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] Guid serviceCenterId,
            [FromQuery] DateTime date
        )
        {
            var slots = await _service.GetAvailableSlotsAsync(serviceCenterId, date);
            return Ok(
                ApiResponse<IReadOnlyList<string>>.SuccessResponse(
                    slots,
                    "Lấy khung giờ khả dụng thành công"
                )
            );
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_STAFF")]
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
            var data = await _service.GetPagedAsync(
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
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<AppointmentResponse>.SuccessResponse(item, "Lấy lịch hẹn thành công")
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> Create([FromBody] AppointmentRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo lịch hẹn thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật lịch hẹn thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá lịch hẹn thành công"));
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> Approve(Guid id, [FromQuery] Guid staffId)
        {
            await _service.ApproveAsync(id, staffId);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Duyệt lịch hẹn thành công"));
        }

        [HttpGet("{id}/getcode")]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> GetCheckinCode(Guid id)
        {
            var code = await _service.GetCheckinCodeAsync(id);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { code }, "Lấy mã check-in thành công")
            );
        }

        [HttpPost("checkin/by-code")]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> CheckInByCode([FromBody] CheckInRequest req)
        {
            await _service.CheckInByCodeAsync(req.Code);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Check-in thành công"));
        }
    }
}
