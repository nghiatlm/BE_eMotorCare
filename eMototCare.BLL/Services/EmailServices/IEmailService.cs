


namespace eMototCare.BLL.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendLoginEmailAsync(string to, string subject, string otpCode);
    }
}
