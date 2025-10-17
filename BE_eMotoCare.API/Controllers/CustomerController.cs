using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create(
            [FromBody] CustomerRequest req
        )
        {
            var id = await _customerService.CreateAsync(req);

            var resp = ApiResponse<object>.CreatedSuccess(new { id }, "tạo thành công");
            return StatusCode((int)resp.StatusCode, resp);
        }
    }
}
