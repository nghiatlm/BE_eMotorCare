using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.DTO.Responses.Labels;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.PartServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/parts")]
    [ApiController]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _partService;
        private readonly INotifierService _notifier;

        public PartsController(IPartService partService, INotifierService notifier)
        {
            _partService = partService;
            _notifier = notifier;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? partTypeId,
            [FromQuery] string? code,
            [FromQuery] string? name,
            [FromQuery] Status? status,
            [FromQuery] int? quantity,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _partService.GetPagedAsync(
                partTypeId,
                code,
                name,
                status,
                quantity,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<PartResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Part thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_TECHNICIAN,ROLE_ADMIN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _partService.GetByIdAsync(id);
            return Ok(ApiResponse<PartResponse>.SuccessResponse(item, "Lấy Part thành công"));
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER")]
        public async Task<IActionResult> Create([FromBody] PartRequest request)
        {
            var id = await _partService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo Part thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _partService.DeleteAsync(id);
            await _notifier.NotifyDeleteAsync("Part", new { Id = id });
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Part thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PartUpdateRequest request)
        {
            await _partService.UpdateAsync(id, request);
            await _notifier.NotifyUpdateAsync("Part", new { Id = id });
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật Part thành công"));
        }

        [HttpGet("by-part-type/{partTypeId}")]
        public async Task<IActionResult> GetByPartType(Guid partTypeId)
        {
            var items = await _partService.GetByPartType(partTypeId);
            return items != null
                ? Ok(
                    ApiResponse<List<PartLabel>>.SuccessResponse(
                        items,
                        "Lấy danh sách Part theo loại thành công"
                    )
                )
                : NotFound(ApiResponse<string>.BadRequest("Không tìm thấy Part theo loại"));
        }

        [HttpGet("by-model-and-type")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetPartsByModelandType(
            [FromQuery] Guid model,
            [FromQuery] Guid partTypeId
        )
        {
            var items = await _partService.GetPartsByModelandType(model, partTypeId);
            return items != null
                ? Ok(
                    ApiResponse<List<PartLabel>>.SuccessResponse(
                        items,
                        "Lấy danh sách Part theo model và loại thành công"
                    )
                )
                : NotFound(
                    ApiResponse<string>.BadRequest("Không tìm thấy Part theo model và loại")
                );
        }
    }
}
