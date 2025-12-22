using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMototCare.BLL.Services.ExportNoteDetailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/export-note-details")]
    [ApiController]
    public class ExportNoteDetailsController : ControllerBase
    {
        private readonly IExportNoteDetailService _service;
        private readonly INotifierExportNoteService _notifier;

        public ExportNoteDetailsController(IExportNoteDetailService service, INotifierExportNoteService notifier)
        {
            _service = service;
            _notifier = notifier;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] ExportNoteDetailUpdateRequest request
        )
        {
            await _service.UpdateAsync(id, request);
            await _notifier.NotifyUpdateAsync("Export Note Detail", new { Id = id, Status = request.Status });
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật thành công"));
        }

        [HttpGet("export-status")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> GetExportStatuses(string appointmentCode, Guid proposedPartId)
        {
            var status = await _service.GetExportStatus(appointmentCode, proposedPartId);
            return Ok(ApiResponse<string>.SuccessResponse(status));
        }
    }
}
