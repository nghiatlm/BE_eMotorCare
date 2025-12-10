


namespace eMototCare.BLL.Services.EmailServices
{
    public interface IEmailService
    {
        Task SendAccountInfo(string to, string subject, string verifyUrl);
        Task SendLoginEmailAsync(string to, string subject, string verifyUrl);
    }
}
