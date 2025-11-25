using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.EVCheckServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/evchecks")]
    [ApiController]
    public class EVChecksController : ControllerBase
    {
        private readonly IEVCheckService _evCheckService;
        private readonly INotifierService _notifier;

        public EVChecksController(IEVCheckService evCheckService, INotifierService notifier)
        {
            _evCheckService = evCheckService;
            _notifier = notifier;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] EVCheckStatus? status,
            [FromQuery] Guid? appointmentId,
            [FromQuery] Guid? taskExecutorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _evCheckService.GetPagedAsync(
                startDate,
                endDate,
                status,
                appointmentId,
                taskExecutorId,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<EVCheckResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách EVCheck thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN")]
        public async Task<IActionResult> Create([FromBody] EVCheckRequest request)
        {
            var id = await _evCheckService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo EVCheck thành công"));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _evCheckService.GetByIdAsync(id);
            return Ok(ApiResponse<EVCheckResponse>.SuccessResponse(item, "Lấy EVCheck thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _evCheckService.DeleteAsync(id);
            await _notifier.NotifyDeleteAsync("EVCheck", new { Id = id });
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá EVCheck thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EVCheckUpdateRequest request)
        {
            await _evCheckService.UpdateAsync(id, request);
            await _notifier.NotifyUpdateAsync(
                "EVCheck",
                new
                {
                    Id = id,
                    Status = request.Status,
                    Action = "UPDATED",
                }
            );
            if (request.Status == EVCheckStatus.QUOTE_APPROVED)
            {
                await _notifier.NotifyUpdateAsync(
                    "Appointment",
                    new { Action = "STATUS_CHANGED", Source = "EV_CHECK_STATUS_CHANGED" }
                );
            }
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật EVCheck thành công"));
        }

        [HttpPut("{id}/approve-quote")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN,ROLE_CUSTOMER")]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _evCheckService.QuoteApprove(id);
            await _notifier.NotifyUpdateAsync("EVCheck", new { Id = id });
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xác nhận sửa chữa"));
        }

        [HttpGet("replacements")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetReplacements([FromQuery] Guid appointmentId)
        {
            var result = await _evCheckService.GetReplacementsByAppointmentAsync(appointmentId);
            return Ok(ApiResponse<List<EVCheckReplacementResponse>>.SuccessResponse(result, "Lấy danh sách bộ phận thay thế thành công"));
        }


    }
}
