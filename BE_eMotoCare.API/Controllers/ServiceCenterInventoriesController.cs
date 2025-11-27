using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.ServiceCenterInventoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/service-center-inventories")]
    [ApiController]
    public class ServiceCenterInventoriesController : ControllerBase
    {
        private readonly IServiceCenterInventoryService _serviceCenterInventoryService;

        public ServiceCenterInventoriesController(IServiceCenterInventoryService serviceCenterInventoryService)
        {
            _serviceCenterInventoryService = serviceCenterInventoryService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? serviceCenterId,
            [FromQuery] string? serviceCenterInventoryName,
            [FromQuery] Status? status,
            [FromQuery] string? partCode,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _serviceCenterInventoryService.GetPagedAsync(serviceCenterId, serviceCenterInventoryName, status, partCode, page, pageSize);
            return Ok(
                ApiResponse<PageResult<ServiceCenterInventoryResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách ServiceCenterInventory thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _serviceCenterInventoryService.GetByIdAsync(id);
            return Ok(
                ApiResponse<ServiceCenterInventoryResponse>.SuccessResponse(
                    item,
                    "Lấy thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] ServiceCenterInventoryRequest request)
        {
            var id = await _serviceCenterInventoryService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo kho thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _serviceCenterInventoryService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Disable kho thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceCenterInventoryUpdateRequest request)
        {
            await _serviceCenterInventoryService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật kho thành công")
            );
        }
    }
}
