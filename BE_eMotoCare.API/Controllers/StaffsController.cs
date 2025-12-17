using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.StaffServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/staffs")]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        [Authorize(Roles = "ROLE_ADMIN,ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] PositionEnum? position,
            [FromQuery] Guid? serviceCenterId,
            [FromQuery] Guid? staffId,
            [FromQuery] Guid? accountId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _staffService.GetPagedAsync(
                search,
                position,
                serviceCenterId,
                staffId,
                accountId,
                page,
                pageSize
            );
            return Ok(
                ApiResponse<PageResult<StaffResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách nhân viên thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_ADMIN,ROLE_STAFF,ROLE_MANAGER,ROLE_TECHNICIAN,ROLE_STOREKEEPER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _staffService.GetByIdAsync(id);
            return Ok(
                ApiResponse<StaffResponse>.SuccessResponse(
                    item,
                    "Lấy thông tin nhân viên thành công"
                )
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_ADMIN,ROLE_MANAGER")]
        public async Task<IActionResult> Create([FromBody] StaffRequest request)
        {
            var id = await _staffService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo nhân viên thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_ADMIN,ROLE_STAFF,ROLE_MANAGER,ROLE_TECHNICIAN,ROLE_STOREKEEPER,ROLE_CUSTOMER")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StaffRequest request)
        {
            await _staffService.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật nhân viên thành công"));
        }
    }
}
