using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.Entities;
using eMototCare.BLL.Services.PayosServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

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

        [HttpPost("create-payment-link/{appointmentId}")]
        public async Task<IActionResult> Checkout(Guid appointmentId)
        {
            var urlPayemt = await _payosService.CreatePaymentAsync(appointmentId);
            if (urlPayemt == null)
            {
                return BadRequest(ApiResponse<string>.BadRequest("Failed to create payment link."));
            }
            return Ok(ApiResponse<object>.SuccessResponse(new { urlPayemt }, "Payment link created successfully"));
        }

        [HttpGet("success")]
        public async Task<IActionResult> Success()
        {
            return Ok(ApiResponse<string>.SuccessResponse("Payment successfully."));
        }

        [HttpGet("failed")]
        public async Task<IActionResult> Failed()
        {
            return Ok(ApiResponse<string>.BadRequest("Payment failed."));
        }

        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment([FromBody] WebhookType type)
        {
            var resullt = await _payosService.VerifyPaymentAsync(type);
            if (resullt)
            {
                return Ok(ApiResponse<string>.SuccessResponse("Payment verified successfully."));
            }
            else
            {
                return BadRequest(ApiResponse<string>.BadRequest("Payment verification failed."));
            }
        }
    }
}
