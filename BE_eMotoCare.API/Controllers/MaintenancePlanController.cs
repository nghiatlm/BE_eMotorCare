using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.MaintenancePlanServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/maintenance-plans")]
    [ApiController]
    public class MaintenancePlanController : ControllerBase
    {
        private readonly IMaintenancePlanService _maintenancePlanService;

        public MaintenancePlanController(IMaintenancePlanService maintenancePlanService)
        {
            _maintenancePlanService = maintenancePlanService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? code,
            [FromQuery] string? description,
            [FromQuery] string? name,
            [FromQuery] int? totalStage,
            [FromQuery] Status? status,
            [FromQuery] MaintenanceUnit[]? maintenanceUnit,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _maintenancePlanService.GetPagedAsync(code, description, name, totalStage, status, maintenanceUnit, page, pageSize);
            return Ok(
                ApiResponse<PageResult<MaintenancePlanResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Maintenance Plan thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF, ROLE_CUSTOMER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _maintenancePlanService.GetByIdAsync(id);
            return Ok(
                ApiResponse<MaintenancePlanResponse>.SuccessResponse(
                    item,
                    "Lấy Maintenance Plan thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Create([FromBody] MaintenancePlanRequest request)
        {
            var id = await _maintenancePlanService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Maintenance Plan thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _maintenancePlanService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Maintenance Plan thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MaintenancePlanRequest request)
        {
            await _maintenancePlanService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Maintenance Plan thành công")
            );
        }
    }
}
