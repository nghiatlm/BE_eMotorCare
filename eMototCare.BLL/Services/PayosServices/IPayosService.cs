


namespace eMototCare.BLL.Services.PayosServices
{
    public interface IPayosService
    {
        Task<string> CreatePaymentAsync(Guid id);
    }
}
