using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using Net.payOS.Types;

namespace eMototCare.BLL.Services.PayosServices
{
    public interface IPayosService
    {
        Task<PayOSCreatePaymentResponse?> CreatePaymentAsync(PaymentRequest request);
        Task<bool> VerifyPaymentAsync(WebhookType type);
    }
}
