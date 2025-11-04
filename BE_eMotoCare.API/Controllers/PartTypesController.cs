using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.PartTypeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/part-types")]
    [ApiController]
    public class PartTypesController : ControllerBase
    {
        private readonly IPartTypeService _partTypeService;

        public PartTypesController(IPartTypeService partTypeService)
        {
            _partTypeService = partTypeService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? name,
            [FromQuery] string? description,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _partTypeService.GetPagedAsync(name, description, page, pageSize);
            return Ok(
                ApiResponse<PageResult<PartTypeResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Part Type thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _partTypeService.GetByIdAsync(id);
            return Ok(
                ApiResponse<PartTypeResponse>.SuccessResponse(item, "Lấy Part Type thành công")
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] PartTypeRequest request)
        {
            var id = await _partTypeService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo Part Type thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _partTypeService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Part Type thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_STOREKEEPER,ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PartTypeUpdateRequest request)
        {
            await _partTypeService.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật Part Type thành công"));
        }
    }
}
