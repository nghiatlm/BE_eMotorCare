using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize(Roles = "ROLE_ADMIN")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
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
            var data = await _accountService.GetPagedAsync(search, role, status, page, pageSize);
            return Ok(
                ApiResponse<PageResult<AccountResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách người dùng thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _accountService.GetByIdAsync(id);
            return Ok(
                ApiResponse<AccountResponse>.SuccessResponse(item, "Lấy người dùng thành công")
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccountRequest request)
        {
            var id = await _accountService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo người dùng thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AccountRequest request)
        {
            await _accountService.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật người dùng thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _accountService.DeleteAsync(id);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Vô hiệu hoá tài khoản thành công")
            );
        }
    }
}
