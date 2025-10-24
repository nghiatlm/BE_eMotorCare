using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.EVCheckServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/evchecks")]
    [ApiController]
    public class EVCheckController : ControllerBase
    {
        private readonly IEVCheckService _evCheckService;

        public EVCheckController(IEVCheckService evCheckService)
        {
            _evCheckService = evCheckService;
        }

        [HttpGet]
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
            var data = await _evCheckService.GetPagedAsync(startDate, endDate, status, appointmentId, taskExecutorId, page, pageSize);
            return Ok(
                ApiResponse<PageResult<EVCheckResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách EVCheck thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EVCheckRequest request)
        {
            var id = await _evCheckService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo EVCheck thành công")
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _evCheckService.GetByIdAsync(id);
            return Ok(
                ApiResponse<EVCheckResponse>.SuccessResponse(
                    item,
                    "Lấy EVCheck thành công"
                )
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _evCheckService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá EVCheck thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EVCheckUpdateRequest request)
        {
            await _evCheckService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật EVCheck thành công")
            );
        }
    }
}
