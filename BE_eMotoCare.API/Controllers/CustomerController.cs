using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;
using eMototCare.BLL.Services.CustomerServices;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByParams(
            [FromQuery] string? firstName,
            [FromQuery] string? lastName,
            [FromQuery] string? address,
            [FromQuery] string? citizenId,
            [FromQuery] Guid? accountId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var data = await _customerService.GetPagedAsync(firstName, lastName, address, citizenId, accountId, page, pageSize);
            return Ok(
                ApiResponse<PageResult<CustomerResponse>>.SuccessResponse(
                    data,
                    "Lấy danh sách Customer thành công"
                )
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _customerService.GetByIdAsync(id);
            return Ok(
                ApiResponse<CustomerResponse>.SuccessResponse(
                    item,
                    "Lấy Customer thành công"
                )
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            var id = await _customerService.CreateAsync(request);
            return Ok(
                ApiResponse<object>.SuccessResponse(new { id }, "Tạo Customer thành công")
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customerService.DeleteAsync(id);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Xoá Customer thành công"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerRequest request)
        {
            await _customerService.UpdateAsync(id, request);
            return Ok(
                ApiResponse<string>.SuccessResponse(null, "Cập nhật Customer thành công")
            );
        }
    }
}   

