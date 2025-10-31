


using Net.payOS.Types;

namespace eMototCare.BLL.Services.PayosServices
{
    public interface IPayosService
    {
        Task<string> CreatePaymentAsync(Guid id);
        Task<bool> VerifyPaymentAsync(WebhookType type);
    }
}
