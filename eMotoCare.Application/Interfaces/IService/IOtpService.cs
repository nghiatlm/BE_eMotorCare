namespace eMotoCare.Application.Interfaces.IService
{
    public interface IOtpService
    {
        Task<string> GenerateAndSendOtpAsync(string phone);
        Task<bool> VerifyOtpAsync(string phone, string code);
    }
}
