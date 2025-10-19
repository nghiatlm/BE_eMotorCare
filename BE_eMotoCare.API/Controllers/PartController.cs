using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.PartServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/parts")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IPartService _partService;

        public PartController(IPartService partService)
        {
            _partService = partService;
        }

        [HttpGet]
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
            var data = await _partService.GetPagedAsync(partTypeId, code, name, status, quantity, page, pageSize);
            return Ok(
                ApiResponse<PageResult<PartResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Part thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _partService.GetByIdAsync(id);
            return Ok(
                ApiResponse<PartResponse>.SuccessResponse(
                    item,
                    "Lấy Part thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartRequest request)
        {
            var id = await _partService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Part thành công")
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _partService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Part thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PartRequest request)
        {
            await _partService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Part thành công")
            );
        }
    }
}
