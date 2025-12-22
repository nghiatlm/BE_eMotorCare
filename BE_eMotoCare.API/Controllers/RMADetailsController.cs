using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.RMADetailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/rma-details")]
    [ApiController]
    public class RMADetailsController : ControllerBase
    {
        private readonly IRMADetailService _service;
        private readonly INotifierRMASerive _notifier;

        public RMADetailsController(IRMADetailService service, INotifierRMASerive notifier)
        {
            _service = service;
            _notifier = notifier;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? rmaNumber,
            [FromQuery] string? inspector,
            [FromQuery] string? result,
            [FromQuery] string? solution,
            [FromQuery] Guid? evCheckDetailId,
            [FromQuery] Guid? rmaId,
            [FromQuery] RMADetailStatus? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(rmaNumber, inspector, result, solution, evCheckDetailId, rmaId, status, page, pageSize);
            return Ok(
                ApiResponse<PageResult<RMADetailResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách RMA Detail thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<RMADetailResponse>.SuccessResponse(
                    item,
                    "Lấy RMA thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] RMADetailRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo RMA Detail thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Disable RMA Detail thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RMADetailUpdateRequest request)
        {
            await _service.UpdateAsync(id, request);
            await _notifier.NotifyUpdateAsync("RMA Detail", new { Id = id, RMADetailStatus = request.Status });
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật RMA Detail thành công")
            );
        }
    }
}
