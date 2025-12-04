
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.FirebaseServices;
using eMototCare.BLL.Services.ProgramService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/programs")]
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _programService;
        private readonly IFirebaseService _firebaseService;

        public ProgramController(IProgramService programService, IFirebaseService firebaseService)
        {
            _programService = programService;
            _firebaseService = firebaseService;
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> CreateProgram([FromBody] ProgramRequest request)
        {
            var result = await _programService.Create(request);
            return result ? Ok(ApiResponse<object>.SuccessResponse(null, "Program created successfully")) :
                            BadRequest(ApiResponse<object>.BadRequest("Failed to create program"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgramById([FromRoute] Guid id)
        {
            var result = await _programService.GetById(id);
            if (result == null)
            {
                return NotFound(ApiResponse<object>.NotFound("Program not found"));
            }
            return Ok(ApiResponse<ProgramDetailResponse>.SuccessResponse(result, "Program retrieved successfully"));
        }

        [HttpGet]
        public async Task<IActionResult> GetProgramsPaged([FromQuery] string? query, [FromQuery] DateTime? startDate,
                                                          [FromQuery] DateTime? endDate, [FromQuery] ProgramType? type,
                                                          [FromQuery] Status? status, [FromQuery] Guid? modelId,
                                                          [FromQuery] int pageCurrent = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _programService.GetPaged(query, startDate, endDate, type, status, modelId, pageCurrent, pageSize);
            return Ok(ApiResponse<PageResult<ProgramResponse>>.SuccessResponse(result, "Programs retrieved successfully"));
        }

        [HttpPost("sync-data")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN")]
        public async Task<IActionResult> SyncProgramData()
        {
            var program = await _firebaseService.GetProgramAsync();
            if (!program)
                return BadRequest(ApiResponse<object>.BadRequest("Đồng bộ program thất bại"));
            var detail = await _firebaseService.GetProgramDetailAsync();
            if (!detail)
                return BadRequest(ApiResponse<object>.BadRequest("Đồng bộ program detail thất bại"));
            return Ok(ApiResponse<object>.SuccessResponse(null, "Đồng bộ program thành công"));
        }

    }
}