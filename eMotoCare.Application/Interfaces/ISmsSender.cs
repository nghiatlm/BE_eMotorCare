

namespace eMotoCare.Application.Interfaces
{
    public interface ISmsSender
    {
        Task SendOtpAsync(string phoneNumber, string otp);
    }
}
