using eMotoCare.BO.DTO.Requests;
using Net.payOS.Types;

namespace eMototCare.BLL.Services.PayosServices
{
    public interface IPayosService
    {
        Task<string> CreatePaymentAsync(PaymentRequest request);
        Task<bool> VerifyPaymentAsync(WebhookType type);
    }
}
