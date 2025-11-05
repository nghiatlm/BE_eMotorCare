using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.PartItemServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/part-items")]
    [ApiController]
    public class PartItemsController : ControllerBase
    {
        private readonly IPartItemService _partItemService;
        public PartItemsController(IPartItemService partItemService)
        {
            _partItemService = partItemService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? partId,
            [FromQuery] Guid? exportNoteId,
            [FromQuery] Guid? importNoteId,
            [FromQuery] string? serialNumber,
            [FromQuery] PartItemStatus? status,
            [FromQuery] Guid? serviceCenterInventoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _partItemService.GetPagedAsync(partId, exportNoteId, importNoteId, serialNumber, status, serviceCenterInventoryId, page, pageSize);
            return Ok(
                ApiResponse<PageResult<PartItemResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Part Item thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _partItemService.GetByIdAsync(id);
            return Ok(
                ApiResponse<PartItemResponse>.SuccessResponse(
                    item,
                    "Lấy PartItem thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_TECHNICIAN")]
        public async Task<IActionResult> Create([FromBody] PartItemRequest request)
        {
            var id = await _partItemService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Part Item thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_TECHNICIAN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _partItemService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Part Item thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_TECHNICIAN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PartItemUpdateRequest request)
        {
            await _partItemService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Part Item thành công")
            );
        }

        [HttpGet("{serviceCenterId}/part-items")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetPartItemsByServiceCenter(Guid serviceCenterId)
        {
            var partItems = await _partItemService.GetByServiceCenterIdAsync(serviceCenterId);

            if (!partItems.Any())
                return NotFound(ApiResponse<string>.NotFound("Không tìm thấy part item"));

            return Ok(ApiResponse<List<PartItemResponse>>.SuccessResponse(partItems, "Lấy danh sách thành công"));
        }
    }
}
