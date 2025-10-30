using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.CustomerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [ApiController]
    [Route("api/v1/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var payload = await _customerService.GetPagedViewAsync(search, page, pageSize);
            return Ok(
                ApiResponse<object>.SuccessResponse(
                    payload,
                    "Lấy danh sách khách hàng (kèm xe/lịch/lịch sử/thay thế) thành công"
                )
            );
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _customerService.GetByIdAsync(id);
            return Ok(
                ApiResponse<CustomerResponse>.SuccessResponse(item, "Lấy khách hàng thành công")
            );
        }

        [HttpPost]
        //[Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_USER")]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            var id = await _customerService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo khách hàng thành công"));
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerRequest request)
        {
            await _customerService.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật khách hàng thành công"));
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "ROLE_MANAGER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customerService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá khách hàng thành công"));
        }
    }
}
