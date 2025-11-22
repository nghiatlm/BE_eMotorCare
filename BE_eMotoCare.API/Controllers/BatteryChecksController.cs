using BE_eMotoCare.API.Extensions;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Responses;
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
