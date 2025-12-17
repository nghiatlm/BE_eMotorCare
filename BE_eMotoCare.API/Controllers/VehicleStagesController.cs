using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.FirebaseServices;
using eMototCare.BLL.Services.VehicleStageServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/vehicle-stages")]
    public class VehicleStagesController : ControllerBase
    {
        private readonly IVehicleStageService _vehicleStageService;
        private readonly IFirebaseService _firebaseService;

        public VehicleStagesController(IVehicleStageService vehicleStageService, IFirebaseService firebaseService)
        {
            _vehicleStageService = vehicleStageService;
            _firebaseService = firebaseService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] Guid? vehicleId,
            [FromQuery] Guid? maintenanceStageId,
            [FromQuery] VehicleStageStatus? status,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _vehicleStageService.GetPagedAsync(
                vehicleId,
                maintenanceStageId,
                status,
                fromDate,
                toDate,
                page,
                pageSize
            );

            return Ok(
                ApiResponse<PageResult<VehicleStageResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách mốc bảo dưỡng thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _vehicleStageService.GetByIdAsync(id);
            return Ok(
                ApiResponse<VehicleStageResponse>.SuccessResponse(
                    item,
                    "Lấy mốc bảo dưỡng thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> Create([FromBody] VehicleStageRequest req)
        {
            var id = await _vehicleStageService.CreateAsync(req);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo mốc bảo dưỡng thành công")
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] VehicleStageRequest req)
        {
            await _vehicleStageService.UpdateAsync(id, req);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật mốc bảo dưỡng thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_STAFF, ROLE_ADMIN, ROLE_CUSTOMER, ROLE_TECHNICIAN, ROLE_MANAGER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _vehicleStageService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá mốc bảo dưỡng thành công"));
        }

        [HttpPost("sync-data")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN")]
        public async Task<IActionResult> SyncVehicleStageData()
        {
            var result = await _firebaseService.GetVehicleStageAsync();
            if (!result)
                return BadRequest(ApiResponse<string>.BadRequest("Đồng bộ dữ liệu thất bại."));
            return Ok(ApiResponse<string>.SuccessResponse(null, "Đồng bộ dữ liệu thành công."));
        }
    }
}
