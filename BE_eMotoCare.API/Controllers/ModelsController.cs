using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.FirebaseServices;
using eMototCare.BLL.Services.ModelServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/models")]
    public class ModelsController : ControllerBase
    {
        private readonly IModelService _modelService;
        private readonly IFirebaseService _firebaseService;

        public ModelsController(IModelService modelService, IFirebaseService firebaseService)
        {
            _modelService = modelService;
            _firebaseService = firebaseService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] Status? status,
            [FromQuery] Guid? modelId,
            [FromQuery] Guid? maintenancePlanId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _modelService.GetPagedAsync(
                search,
                status,
                modelId,
                maintenancePlanId,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<ModelResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách model thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_ADMIN,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _modelService.GetByIdAsync(id);
            return Ok(
                ApiResponse<ModelResponse>.SuccessResponse(item, "Lấy chi tiết model thành công")
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] ModelRequest request)
        {
            var id = await _modelService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo model thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ModelUpdateRequest request)
        {
            await _modelService.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật model thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _modelService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Vô hiệu hóa model thành công"));
        }

        [HttpPost("sync-model")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_ADMIN,ROLE_STAFF")]
        public async Task<IActionResult> Sync()
        {
            var model = await _firebaseService.GetModelAsync();
            return Ok(
                ApiResponse<string>.SuccessResponse(
                    "Đồng bộ model từ Firebase thành công"
                )
            );
        }
    }
}
