namespace eMotoCare.BLL.Services.OtpServices
{
    public interface IOtpService
    {
        Task<string> GenerateAndSendOtpAsync(string phone);
        Task<bool> VerifyOtpAsync(string phone, string code);
    }
}
