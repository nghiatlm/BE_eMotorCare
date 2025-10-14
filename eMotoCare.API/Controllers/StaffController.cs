using eMotoCare.BLL.Services.StaffService;
using eMotoCare.Common.Enums;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.ApiResponse;
using eMotoCare.Common.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/staffs")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class StaffManagementController : ControllerBase
    {
        private readonly IStaffService _service;

        public StaffManagementController(IStaffService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] Gender? gender,
            [FromQuery] StaffPosition? position,
            [FromQuery] Guid? branchId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(
                search,
                gender,
                position,
                branchId,
                page,
                pageSize
            );

            if (
                data == null
                || data.RowDatas == null
                || data.RowDatas.Count == 0
                || data.Total == 0
            )
                throw new AppException(ErrorCode.LIST_EMPTY);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Get staffs successfully",
                    Data = data,
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var staff = await _service.GetByIdAsync(id);
            if (staff is null)
                throw new AppException(ErrorCode.NOT_FOUND);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Get staff successfully",
                    Data = staff,
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StaffRequest req)
        {
            var id = await _service.CreateAsync(req);
            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Staff created",
                    Data = new { id },
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StaffRequest req)
        {
            await _service.UpdateAsync(id, req);
            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Staff updated",
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Staff deleted",
                }
            );
        }
    }
}
