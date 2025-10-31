using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.MaintenanceStageServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/maintenance-stages")]
    [ApiController]
    public class MaintenanceStagesController : ControllerBase
    {
        private readonly IMaintenanceStageService _maintenanceStageService;

        public MaintenanceStagesController(IMaintenanceStageService maintenanceStageService)
        {
            _maintenanceStageService = maintenanceStageService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? maintenancePlanId,
            [FromQuery] string? description,
            [FromQuery] DurationMonth? durationMonth,
            [FromQuery] Mileage? mileage,
            [FromQuery] string? name,
            [FromQuery] Status? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _maintenanceStageService.GetPagedAsync(maintenancePlanId, description, durationMonth, mileage, name, status, page, pageSize);
            return Ok(
                ApiResponse<PageResult<MaintenanceStageResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Maintenance Stage thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _maintenanceStageService.GetByIdAsync(id);
            return Ok(
                ApiResponse<MaintenanceStageResponse>.SuccessResponse(
                    item,
                    "Lấy Maintenance Stage thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Create([FromBody] MaintenanceStageRequest request)
        {
            var id = await _maintenanceStageService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Maintenance Stage thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _maintenanceStageService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Maintenance Stage thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MaintenanceStageUpdateRequest request)
        {
            await _maintenanceStageService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Maintenance Stage thành công")
            );
        }
    }
}
