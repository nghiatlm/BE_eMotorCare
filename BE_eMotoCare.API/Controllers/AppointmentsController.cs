using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.AppointmentServices;
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
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<AppointmentResponse>.SuccessResponse(item, "Lấy lịch hẹn thành công")
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo lịch hẹn thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AppointmentRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật lịch hẹn thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá lịch hẹn thành công"));
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id, [FromQuery] Guid staffId)
        {
            await _service.ApproveAsync(id, staffId);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Duyệt lịch hẹn thành công"));
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] AppointmentStatus status)
        {
            await _service.UpdateStatusAsync(id, status);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật trạng thái thành công"));
        }

        [HttpGet("{id}/checkin")]
        public async Task<IActionResult> GetCheckinCode(Guid id)
        {
            var code = await _service.GetCheckinCodeAsync(id);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { code }, "Lấy mã check-in thành công")
            );
        }

        [HttpPost("assign-technician")]
        public async Task<IActionResult> AssignTechnician(
            Guid id,
            [FromBody] AssignTechnicianRequest req,
            [FromQuery] Guid approveById
        )
        {
            await _service.AssignTechnicianAsync(id, req.TechnicianId, approveById);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Gán kỹ thuật viên thành công"));
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn(Guid id)
        {
            await _service.CheckInAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Check-in thành công"));
        }

        [HttpPost("inspection")]
        public async Task<IActionResult> UpsertInspection(
            Guid id,
            [FromBody] EVCheckUpsertRequest req,
            [FromQuery] Guid technicianId
        )
        {
            var res = await _service.UpsertEVCheckAsync(id, req, technicianId);
            return Ok(
                ApiResponse<EVCheckResponse>.SuccessResponse(res, "Lưu kết quả kiểm tra thành công")
            );
        }

        [HttpGet("inspection")]
        public async Task<IActionResult> GetInspection(Guid id)
        {
            var res = await _service.GetEVCheckAsync(id);
            return Ok(
                ApiResponse<EVCheckResponse?>.SuccessResponse(
                    res,
                    "Lấy kết quả kiểm tra thành công"
                )
            );
        }

        [HttpPost("inspection/confirm")]
        public async Task<IActionResult> ConfirmInspection(
            Guid id,
            [FromBody] InspectionConfirmRequest req
        )
        {
            await _service.ConfirmInspectionAsync(id, req);
            return Ok(
                ApiResponse<string>.SuccessResponse(
                    null,
                    req.Accept ? "Khách đã xác nhận" : "Khách đã hủy"
                )
            );
        }

        [HttpPost("repair/start")]
        public async Task<IActionResult> StartRepair(Guid id)
        {
            await _service.StartRepairAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Đã bắt đầu sửa chữa"));
        }

        [HttpPost("repair/finish")]
        public async Task<IActionResult> FinishRepair(Guid id)
        {
            await _service.FinishRepairAsync(id);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Đã hoàn tất sửa chữa, chờ thanh toán")
            );
        }

        [HttpGet("repair-ticket")]
        public async Task<IActionResult> GetRepairTicket(Guid id)
        {
            var res = await _service.GetRepairTicketAsync(id);
            return Ok(
                ApiResponse<RepairTicketResponse>.SuccessResponse(
                    res,
                    "Lấy phiếu sửa chữa thành công"
                )
            );
        }
    }
}
