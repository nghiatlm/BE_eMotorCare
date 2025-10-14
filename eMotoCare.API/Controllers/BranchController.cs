using eMotoCare.BLL.Services.BranchServices;
using eMotoCare.Common.Enums;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.ApiResponse;
using eMotoCare.Common.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/branches")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;

        public BranchController(IBranchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] Status? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _service.GetPagedAsync(search, status, page, pageSize);

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
                    Message = "Get branches successfully",
                    Data = data,
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var branch = await _service.GetByIdAsync(id);
            if (branch is null)
                throw new AppException(ErrorCode.NOT_FOUND);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Get branch successfully",
                    Data = branch,
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchRequest req)
        {
            var id = await _service.CreateAsync(req);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Branch created",
                    Data = new { id },
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BranchRequest req)
        {
            await _service.UpdateAsync(id, req);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Branch updated",
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
                    Message = "Branch deleted",
                }
            );
        }
    }
}
