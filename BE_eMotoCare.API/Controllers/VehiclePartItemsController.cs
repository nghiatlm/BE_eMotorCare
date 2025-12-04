using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.FirebaseServices;
using eMototCare.BLL.Services.VehiclePartItemServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/vehicle-part-items")]
    public class VehiclePartItemsController : ControllerBase
    {
        private readonly IVehiclePartItemService _vehiclePartItemService;
        private readonly IFirebaseService _firebaseService;

        public VehiclePartItemsController(IVehiclePartItemService vehiclePartItemService, IFirebaseService firebaseService)
        {
            _vehiclePartItemService = vehiclePartItemService;
            _firebaseService = firebaseService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
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
            var data = await _vehiclePartItemService.GetPagedAsync(
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
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _vehiclePartItemService.GetByIdAsync(id);
            return Ok(
                ApiResponse<VehiclePartItemResponse>.SuccessResponse(
                    item,
                    "Lấy chi tiết thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> Create([FromBody] VehiclePartItemRequest request)
        {
            var id = await _vehiclePartItemService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehiclePartItemRequest request)
        {
            await _vehiclePartItemService.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _vehiclePartItemService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá thành công"));
        }

        [HttpPost("sync-data")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN")]
        public async Task<IActionResult> SyncMaintenanceData()
        {
            var result = await _firebaseService.GetVehiclePartitemAsync();
            if (!result)
            return BadRequest(ApiResponse<string>.BadRequest("Đồng bộ thất bại"));

            return Ok(ApiResponse<string>.SuccessResponse("Đồng bộ vehicle part item thành công."));
        }
    }
}
