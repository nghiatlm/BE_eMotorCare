using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.Entities;
using eMototCare.BLL.Services.PayosServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_eMotoCare.API.Controllers
{
    [Route("api/v1/checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IPayosService _payosService;
        public CheckoutController(IPayosService payosService)
        {
            _payosService = payosService;
        }
        [HttpPost("create-payment-link/{evCheckId}")]
        public async Task<IActionResult> Checkout(Guid evCheckId)
        {
            var urlPayemt = await _payosService.CreatePaymentAsync(evCheckId);
            if (urlPayemt == null)
            {
                return BadRequest(ApiResponse<string>.SuccessResponse("Failed to create payment link."));
            }
            return Ok(ApiResponse<object>.SuccessResponse(new { urlPayemt }, "Tạo payment link thành công"));
        }
    }
}
