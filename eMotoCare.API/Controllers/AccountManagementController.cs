using eMotoCare.BLL.Services.AdminServices;
using eMotoCare.Common.Enums;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            return Ok(
                new ApiResponse
                {
                    Code = 200,
                    Success = true,
                    Message = "Get users successfully",
                    Data = data,
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _adminUserService.GetByIdAsync(id);
            if (user is null)
                return NotFound(
                    new ApiResponse
                    {
                        Code = 404,
                        Success = false,
                        Message = "User not found",
                    }
                );

            return Ok(
                new ApiResponse
                {
                    Code = 200,
                    Success = true,
                    Data = user,
                }
            );
        }

        public record CreateUserParams(
            string Phone,
            string Password,
            string FullName,
            RoleName Role,
            string? Email,
            string? AvatarUrl
        );

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserParams req)
        {
            try
            {
                var id = await _adminUserService.CreateAsync(
                    req.Phone,
                    req.Password,
                    req.FullName,
                    req.Role,
                    req.Email,
                    req.AvatarUrl
                );
                return Ok(
                    new ApiResponse
                    {
                        Code = 200,
                        Success = true,
                        Message = "User created",
                        Data = new { id },
                    }
                );
            }
            catch (AppException ex)
            {
                return BadRequest(
                    new ApiResponse
                    {
                        Code = 400,
                        Success = false,
                        Message = ex.Message,
                    }
                );
            }
        }

        public record UpdateUserParams(
            string? FullName,
            RoleName? Role,
            string? Email,
            AccountStatus? Status
        );

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserParams req)
        {
            try
            {
                await _adminUserService.UpdateAsync(
                    id,
                    req.FullName,
                    req.Role,
                    req.Email,
                    req.Status
                );
                return Ok(
                    new ApiResponse
                    {
                        Code = 200,
                        Success = true,
                        Message = "User updated",
                    }
                );
            }
            catch (AppException ex)
            {
                return BadRequest(
                    new ApiResponse
                    {
                        Code = 400,
                        Success = false,
                        Message = ex.Message,
                    }
                );
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _adminUserService.DeleteAsync(id);
                return Ok(
                    new ApiResponse
                    {
                        Code = 200,
                        Success = true,
                        Message = "User deleted",
                    }
                );
            }
            catch (AppException ex)
            {
                return BadRequest(
                    new ApiResponse
                    {
                        Code = 400,
                        Success = false,
                        Message = ex.Message,
                    }
                );
            }
        }
    }
}
