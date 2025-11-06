using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.ExportNoteDetailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/export-note-details")]
    [ApiController]
    public class ExportNoteDetailsController : ControllerBase
    {
        private readonly IExportNoteDetailService _exportNoteDetailService;

        public ExportNoteDetailsController(IExportNoteDetailService exportNoteDetailService)
        {
            _exportNoteDetailService = exportNoteDetailService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] Guid? exportNoteId,
            [FromQuery] Guid? partItemId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _exportNoteDetailService.GetPagedAsync(exportNoteId, partItemId, page, pageSize);
            return Ok(
                ApiResponse<PageResult<ExportNoteDetailResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Export note detail thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _exportNoteDetailService.GetByIdAsync(id);
            return Ok(
                ApiResponse<ExportNoteDetailResponse>.SuccessResponse(
                    item,
                    "Lấy Export Note Detail thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> Create([FromBody] ExportNoteDetailRequest request)
        {
            var id = await _exportNoteDetailService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Export Note Detail thành công")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _exportNoteDetailService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Export Note Detail thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExportNoteDetailRequest request)
        {
            await _exportNoteDetailService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Part thành công")
            );
        }
    }
}
