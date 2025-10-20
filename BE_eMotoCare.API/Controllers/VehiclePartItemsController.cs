using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.VehiclePartItemServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/vehicle-part-items")]
    public class VehiclePartItemsController : ControllerBase
    {
        private readonly IVehiclePartItemService _service;

        public VehiclePartItemsController(IVehiclePartItemService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] Guid? vehicleId,
            [FromQuery] Guid? partItemId,
            [FromQuery] Guid? replaceForId,
            [FromQuery] DateTime? fromInstallDate,
            [FromQuery] DateTime? toInstallDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(
                search,
                vehicleId,
                partItemId,
                replaceForId,
                fromInstallDate,
                toInstallDate,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<VehiclePartItemResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách linh kiện gắn xe thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<VehiclePartItemResponse>.SuccessResponse(
                    item,
                    "Lấy chi tiết thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehiclePartItemRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehiclePartItemRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá thành công"));
        }
    }
}
