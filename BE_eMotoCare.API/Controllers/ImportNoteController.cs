using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.ImportNoteServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/import-notes")]
    [ApiController]
    public class ImportNoteController : ControllerBase
    {
        private readonly IImportNoteService _importNoteService;

        public ImportNoteController(IImportNoteService importNoteService)
        {
            _importNoteService = importNoteService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? code,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? importFrom,
            [FromQuery] string? supplier,
            [FromQuery] ImportType? importType,
            [FromQuery] decimal? totalAmount,
            [FromQuery] Guid? importById,
            [FromQuery] Guid? serviceCenterId,
            [FromQuery] ImportNoteStatus? importNoteStatus,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _importNoteService.GetPagedAsync(
                code, 
                fromDate, 
                toDate, 
                importFrom, 
                supplier,
                importType,
                totalAmount,
                importById, 
                serviceCenterId, 
                importNoteStatus,
                page, 
                pageSize);
            return Ok(
                ApiResponse<PageResult<ImportNoteResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Import Note thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _importNoteService.GetByIdAsync(id);
            return Ok(
                ApiResponse<ImportNoteResponse>.SuccessResponse(
                    item,
                    "Lấy Import Note thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Create([FromBody] ImportNoteRequest request)
        {
            var id = await _importNoteService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Import Note thành công, status: RECEIVING")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _importNoteService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Disable Import Note thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ImportNoteUpdateRequest request)
        {
            await _importNoteService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Import Note thành công")
            );
        }
    }
}
