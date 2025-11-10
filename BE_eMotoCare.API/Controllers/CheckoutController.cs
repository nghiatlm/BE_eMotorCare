using BE_eMotoCare.API.Realtime.Services;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
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
        private readonly INotifierService _notifier;

        public CheckoutController(IPayosService payosService, INotifierService notifier)
        {
            _payosService = payosService;
            _notifier = notifier;
        }

        [HttpPost("create-payment-link")]
        public async Task<IActionResult> Checkout([FromBody] PaymentRequest request)
        {
            var urlPayemt = await _payosService.CreatePaymentAsync(request);
            if (urlPayemt == null)
            {
                return BadRequest(ApiResponse<string>.BadRequest("Failed to create payment link."));
            }
            return Ok(
                ApiResponse<object>.SuccessResponse(
                    new
                    {
                        checkoutUrl = urlPayemt.CheckoutUrl,
                        transactionCode = urlPayemt.TransactionCode,
                    },
                    urlPayemt.CheckoutUrl is null
                        ? "Payment created successfully."
                        : "Payment link created successfully."
                )
            );
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
                await _notifier.NotifyUpdateAsync(
                    "Payment",
                    new
                    {
                        Message = "Payment verified successfully.",
                        OrderCode = type.code,
                        Status = "Success",
                    }
                );
                return Ok(ApiResponse<string>.SuccessResponse("Payment verified successfully."));
            }
            else
            {
                await _notifier.NotifyUpdateAsync(
                    "Payment",
                    new
                    {
                        Message = "Payment verification failed.",
                        OrderCode = type.code,
                        Status = "Failed",
                    }
                );
                return BadRequest(ApiResponse<string>.BadRequest("Payment verification failed."));
            }
        }
    }
}
