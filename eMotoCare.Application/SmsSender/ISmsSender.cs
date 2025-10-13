namespace eMotoCare.BLL.SmsSender
{
    public interface ISmsSender
    {
        Task SendOtpAsync(string phoneNumber, string otp);
    }
}
