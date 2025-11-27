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

        public ExportNoteDetailsController(IExportNoteDetailService service)
        {
            _service = service;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] ExportNoteDetailUpdateRequest request
        )
        {
            await _service.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật thành công"));
        }
    }
}
