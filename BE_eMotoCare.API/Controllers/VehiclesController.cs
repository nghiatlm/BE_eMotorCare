using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.VehicleServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _service;

        public VehiclesController(IVehicleService service) => _service = service;

        [HttpGet]
        [Authorize(Roles = "ROLE_STAFF")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] StatusEnum? status,
            [FromQuery] Guid? modelId,
            [FromQuery] Guid? customerId,
            [FromQuery] DateTime? fromPurchaseDate,
            [FromQuery] DateTime? toPurchaseDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(
                search,
                status,
                modelId,
                customerId,
                fromPurchaseDate,
                toPurchaseDate,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<VehicleResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách xe thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<VehicleResponse>.SuccessResponse(item, "Lấy xe thành công"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo xe thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehicleRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật xe thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá xe thành công"));
        }
    }
}
