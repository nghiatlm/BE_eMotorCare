using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.ModelPartTypeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/model-part-types")]
    [ApiController]
    public class ModelPartTypesController : ControllerBase
    {
        private readonly IModelPartTypeService _service;

        public ModelPartTypesController(IModelPartTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] Status? status,
            [FromQuery] Guid? id,
            [FromQuery] Guid? modelId,
            [FromQuery] Guid? partTypeId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var result = await _service.GetPagedAsync(
                search,
                status,
                id,
                modelId,
                partTypeId,
                page,
                pageSize
            );

            return Ok(
                ApiResponse<PageResult<ModelPartTypeResponse>>.SuccessResponse(
                    result,
                    "Lấy danh sách ModelPartType thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<ModelPartTypeResponse>.SuccessResponse(
                    data,
                    "Lấy chi tiết ModelPartType thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Create([FromBody] ModelPartTypeRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(ApiResponse<Guid>.SuccessResponse(id, "Tạo ModelPartType thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] ModelPartTypeUpdateRequest request
        )
        {
            await _service.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse("OK", "Cập nhật ModelPartType thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(
                ApiResponse<string>.SuccessResponse("OK", "Vô hiệu hoá ModelPartType thành công")
            );
        }
    }
}
