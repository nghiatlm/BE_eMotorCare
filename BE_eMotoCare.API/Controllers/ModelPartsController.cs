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
    public class ModelPartsController : ControllerBase
    {
        private readonly IModelPartService _modelPartService;

        public ModelPartsController(IModelPartService modelPartService)
        {
            _modelPartService = modelPartService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] Status? status,
            [FromQuery] Guid? id,
            [FromQuery] Guid? modelId,
            [FromQuery] Guid? partId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var result = await _modelPartService.GetPagedAsync(
                search,
                status,
                id,
                modelId,
                partId,
                page,
                pageSize
            );

            return Ok(
                ApiResponse<PageResult<ModelPartResponse>>.SuccessResponse(
                    result,
                    "Lấy danh sách ModelPart thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _modelPartService.GetByIdAsync(id);
            return Ok(
                ApiResponse<ModelPartResponse>.SuccessResponse(
                    data,
                    "Lấy chi tiết ModelPartType thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Create([FromBody] ModelPartRequest request)
        {
            var id = await _modelPartService.CreateAsync(request);
            return Ok(ApiResponse<Guid>.SuccessResponse(id, "Tạo ModelPartType thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ModelPartUpdateRequest request)
        {
            await _modelPartService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse("OK", "Cập nhật ModelPartType thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _modelPartService.DeleteAsync(id);
            return Ok(
                ApiResponse<string>.SuccessResponse("OK", "Vô hiệu hoá ModelPartType thành công")
            );
        }
    }
}
