using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.ExportServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/export-notes")]
    [ApiController]
    public class ExportNotesController : ControllerBase
    {
        private readonly IExportService _exportService;
        private readonly INotifierService _notifier;

        public ExportNotesController(IExportService exportService, INotifierService notifier)
        {
            _exportService = exportService;
            _notifier = notifier;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? code,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] ExportType? exportType,
            [FromQuery] string? exportTo,
            [FromQuery] int? totalQuantity,
            [FromQuery] decimal? totalValue,
            [FromQuery] Guid? exportById,
            [FromQuery] Guid? serviceCenterId,
            [FromQuery] ExportNoteStatus? exportNoteStatus,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _exportService.GetPagedAsync(
                code,
                fromDate,
                toDate,
                exportType,
                exportTo,
                totalQuantity,
                totalValue,
                exportById,
                serviceCenterId,
                exportNoteStatus,
                page,
                pageSize);
            return Ok(
                ApiResponse<PageResult<ExportNoteResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Export Note thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _exportService.GetByIdAsync(id);
            return Ok(
                ApiResponse<ExportNoteResponse>.SuccessResponse(
                    item,
                    "Lấy Export Note thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] ExportNoteRequest request)
        {
            var id = await _exportService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Export Note thành công, status: RECEIVING")
            );
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _exportService.DeleteAsync(id);
            await _notifier.NotifyDeleteAsync("Export Note", new { Id = id });
            return Ok(ApiResponse<string>.SuccessResponse(null, "Disable Export Note thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ExportNoteUpdateRequest request)
        {
            await _exportService.UpdateAsync(id, request);
            await _notifier.NotifyUpdateAsync("Export Note", new { Id = id });
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Export Note thành công")
            );
        }
    }
}
