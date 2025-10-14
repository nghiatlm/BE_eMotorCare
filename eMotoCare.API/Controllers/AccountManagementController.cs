using eMotoCare.BLL.Services.AdminServices;
using eMotoCare.Common.Enums;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/admin/users")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class AccountManagementController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AccountManagementController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] RoleName? role,
            [FromQuery] AccountStatus? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _adminUserService.GetPagedAsync(search, role, status, page, pageSize);

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
                    Message = "Get users successfully",
                    Data = data,
                }
            );
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _adminUserService.GetByIdAsync(id);
            if (user is null)
                throw new AppException(ErrorCode.USER_NOT_FOUND);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Get user successfully",
                    Data = user,
                }
            );
        }

        public record CreateUserParams(
            string Phone,
            string Password,
            string FullName,
            RoleName Role,
            string? Email
        );

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserParams req)
        {
            if (string.IsNullOrWhiteSpace(req.Phone) || string.IsNullOrWhiteSpace(req.Password))
                throw new AppException(ErrorCode.NOT_NULL);

            var id = await _adminUserService.CreateAsync(
                req.Phone,
                req.Password,
                req.FullName,
                req.Role,
                req.Email
            );

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "User created",
                    Data = new { id },
                }
            );
        }

        public record UpdateUserParams(
            string? FullName,
            RoleName? Role,
            string? Email,
            AccountStatus? Status
        );

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserParams req)
        {
            await _adminUserService.UpdateAsync(id, req.FullName, req.Role, req.Email, req.Status);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "User updated",
                }
            );
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _adminUserService.DeleteAsync(id);

            return Ok(
                new ApiResponse
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "User deleted",
                }
            );
        }
    }
}
