using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using Microsoft.AspNetCore.Mvc;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.RMAServices;
using Microsoft.AspNetCore.Authorization;
using BE_eMotoCare.API.Realtime.Services;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/rmas")]
    [ApiController]
    public class RMAsController : ControllerBase
    {
        private readonly IRMAService _service;
        private readonly INotifierRMASerive _notifier;

        public RMAsController(IRMAService service, INotifierRMASerive notifier)
        {
            _service = service;
            _notifier = notifier;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? code,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? returnAddress,
            [FromQuery] RMAStatus? status,
            [FromQuery] Guid? createdById,
            [FromQuery] Guid? serviceCenterId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(code,fromDate,toDate,returnAddress,status,createdById, serviceCenterId, page, pageSize);
            return Ok(
                ApiResponse<PageResult<RMAResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách RMA thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<RMAResponse>.SuccessResponse(
                    item,
                    "Lấy RMA thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] RMARequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo RMA thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cancel RMA thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RMAUpdateRequest request)
        {
            await _service.UpdateAsync(id, request);
            await _notifier.NotifyUpdateAsync("RMA", new { Id = id, RMAStatus = request.Status });
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật RMA thành công")
            );
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "ROLE_CUSTOMER,ROLE_MANAGER")]
        public async Task<IActionResult> GetByCustomerId(Guid customerId)
        {
            var rmas = await _service.GetByCustomerIdAsync(customerId);
            if (rmas == null || !rmas.Any())
                return NotFound(ApiResponse<string>.NotFound("Không tìm thấy RMA"));

            return Ok(ApiResponse<List<RMAResponse>>.SuccessResponse(rmas, "Lấy danh sách RMA theo Customer Id thành công"));
        }
    }
}
