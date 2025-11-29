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
        private readonly IVehicleService _vehicleService;

        public VehiclesController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
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
            var data = await _vehicleService.GetPagedAsync(
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
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _vehicleService.GetByIdAsync(id);
            return Ok(ApiResponse<VehicleResponse>.SuccessResponse(item, "Lấy xe thành công"));
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> Create([FromBody] VehicleRequest request)
        {
            var id = await _vehicleService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo xe thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehicleRequest request)
        {
            await _vehicleService.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật xe thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _vehicleService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Vô hiệu hoá xe thành công"));
        }

        [HttpGet("{vehicleId}/history")]
        [Authorize(Roles = "ROLE_CUSTOMER,ROLE_STAFF,ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> GetHistory(Guid vehicleId)
        {
            var data = await _vehicleService.GetHistoryAsync(vehicleId);

            return Ok(
                ApiResponse<VehicleHistoryResponse>.SuccessResponse(
                    data,
                    "Lấy lịch sử xe thành công"
                )
            );
        }

        [HttpPost("sync-vehicles")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> Sync([FromBody] SyncVehicleRequest request)
        {
            var vehicle = await _vehicleService.SyncVehicleAsync(request);
            return Ok(
                ApiResponse<VehicleResponse>.SuccessResponse(
                    vehicle,
                    "Đồng bộ vehicle từ OEM thành công"
                )
            );
        }
    }
}
