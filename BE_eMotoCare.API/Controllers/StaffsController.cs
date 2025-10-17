using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.StaffServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/staffs")]
    //[Authorize(Roles = "ROLE_ADMIN")]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _service;

        public StaffsController(IStaffService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] PositionEnum? position,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(search, position, page, pageSize);
            return Ok(
                ApiResponse<PageResult<StaffResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách nhân viên thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return Ok(
                ApiResponse<StaffResponse>.SuccessResponse(
                    item,
                    "Lấy thông tin nhân viên thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StaffRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo nhân viên thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StaffRequest request)
        {
            await _service.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật nhân viên thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá nhân viên thành công"));
        }
    }
}
