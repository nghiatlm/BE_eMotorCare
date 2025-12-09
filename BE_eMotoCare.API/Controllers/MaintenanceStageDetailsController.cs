using BE_eMotoCare.API.Extensions;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.MaintenanceStageDetailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/maintenance-stage-details")]
    [ApiController]
    public class MaintenanceStageDetailsController : ControllerBase
    {
        private readonly IMaintenanceStageDetailService _maintenanceStageDetailService;

        public MaintenanceStageDetailsController(
            IMaintenanceStageDetailService maintenanceStageDetailService
        )
        {
            _maintenanceStageDetailService = maintenanceStageDetailService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? maintenanceStageId,
            [FromQuery] Guid? partId,
            [FromQuery] ActionType[]? actionType,
            [FromQuery] string? description,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _maintenanceStageDetailService.GetPagedAsync(
                maintenanceStageId,
                partId,
                actionType,
                description,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<MaintenanceStageDetailResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Maintenance Stage Detail thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _maintenanceStageDetailService.GetByIdAsync(id);
            return Ok(
                ApiResponse<MaintenanceStageDetailResponse>.SuccessResponse(
                    item,
                    "Lấy Maintenance Stage Detail thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Create([FromBody] MaintenanceStageDetailRequest request)
        {
            var id = await _maintenanceStageDetailService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(
                    new { id },
                    "Tạo Maintenance Stage Detail thành công"
                )
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _maintenanceStageDetailService.DeleteAsync(id);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Xoá Maintenance Stage Detail thành công")
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] MaintenanceStageDetailUpdateRequest request
        )
        {
            await _maintenanceStageDetailService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(
                    null,
                    "Cập nhật Maintenance Stage Detail thành công"
                )
            );
        }

    }
}
