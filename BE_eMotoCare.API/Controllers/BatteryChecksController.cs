using BE_eMotoCare.API.Extensions;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.BatteryCheckServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/battery-checks")]
    public class BatteryChecksController : ControllerBase
    {
        private readonly IBatteryCheckService _batteryCheckService;

        public BatteryChecksController(IBatteryCheckService batteryCheckService)
        {
            _batteryCheckService = batteryCheckService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_TECHNICIAN,ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] Guid? evCheckDetailId,
            [FromQuery] Guid? vehicleId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? sortBy,
            [FromQuery] bool sortDesc = true,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _batteryCheckService.GetPagedAsync(
                evCheckDetailId,
                vehicleId,
                fromDate,
                toDate,
                sortBy,
                sortDesc,
                page,
                pageSize
            );

            return Ok(
                ApiResponse<PageResult<BatteryCheckAnalysisResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách BatteryCheck thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_TECHNICIAN,ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _batteryCheckService.GetByIdAsync(id);
            return Ok(
                ApiResponse<BatteryCheckAnalysisResponse>.SuccessResponse(
                    data,
                    "Lấy chi tiết BatteryCheck thành công"
                )
            );
        }

        [HttpPost("import")]
        [Authorize(Roles = "ROLE_TECHNICIAN,ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Import([FromForm] BatteryCheckUploadForm req)
        {
            if (req.File == null || req.File.Length == 0)
                return BadRequest(
                    ApiResponse<string>.BadRequest("File upload rỗng hoặc không tồn tại.")
                );

            using var stream = req.File.OpenReadStream();

            var result = await _batteryCheckService.ImportFromCsvAsync(req.EVCheckDetailId, stream);

            return Ok(
                ApiResponse<BatteryCheckAnalysisResponse>.SuccessResponse(
                    result,
                    "Import & phân tích dữ liệu Battery thành công"
                )
            );
        }
    }
}
