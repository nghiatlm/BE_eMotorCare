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
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] string? search,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _customerService.GetPagedAsync(search, page, pageSize);
            return Ok(
                ApiResponse<PageResult<CustomerResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách khách hàng thành công"
                )
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _customerService.GetByIdAsync(id);
            return Ok(
                ApiResponse<CustomerResponse>.SuccessResponse(item, "Lấy khách hàng thành công")
            );
        }

        [HttpGet("account/{accountId}")]
        [Authorize(Roles = "ROLE_CUSTOMER,ROLE_MANAGER,ROLE_STAFF")]
        public async Task<IActionResult> GetAccountIdAsync(Guid accountId)
        {
            var item = await _customerService.GetAccountIdAsync(accountId);
            return Ok(
                ApiResponse<CustomerResponse>.SuccessResponse(item, "Lấy khách hàng thành công")
            );
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER")]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            var id = await _customerService.CreateAsync(request);
            return Ok(ApiResponse<object>.SuccessResponse(new { id }, "Tạo khách hàng thành công"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerRequest request)
        {
            await _customerService.UpdateAsync(id, request);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Cập nhật khách hàng thành công"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ROLE_MANAGER")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customerService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá khách hàng thành công"));
        }

        [HttpGet("rma/{rmaId}")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_TECHNICIAN")]
        public async Task<IActionResult> GetCustomerByRmaId(Guid rmaId)
        {
            var customer = await _customerService.GetCustomerByRmaIdAsync(rmaId);
            if (customer == null)
                return NotFound(ApiResponse<string>.NotFound("Không tìm thấy khách hàng."));

            return Ok(ApiResponse<CustomerResponse>.SuccessResponse(customer, "Lấy khách hàng thành công"));
        }

        [HttpPost("sync-data")]
        [Authorize(Roles = "ROLE_MANAGER,ROLE_STAFF,ROLE_CUSTOMER,ROLE_ADMIN")]
        public async Task<IActionResult> SyncCustomerData([FromBody] CustomerSyncRequest request)
        {
            var result = await _customerService.SyncCustomerAsync(request.AccountId, request.CitizenId);
            if (!result)
                return BadRequest(ApiResponse<string>.BadRequest("Đồng bộ dữ liệu khách hàng thất bại."));
            return Ok(ApiResponse<string>.SuccessResponse(null, "Đồng bộ dữ liệu khách hàng thành công."));
        }
    }
}
