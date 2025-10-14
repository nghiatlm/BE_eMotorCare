using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;

namespace eMotoCare.BLL.Services.AuthenticateService
{
    public interface IAuthenticateService
    {
        public Task<bool> Register(RegisterRequest request);
        public Task<bool> VerifyOtpAsync(string phone, string code);
        public Task<AuthenticateResponse> Login(string email, string password);
        public Task<bool> ChangePassword(string oldPassword, string newPassword, string confirmPassword, Guid id);
        public Task<ForgetPasswordResponse> ForgotPasswordAsync(string phone);
        public Task<ForgetPasswordResponse> ResetPasswordAsync(string phoneNumber, string Otp, string newPassword, string confirmPassword);

    }
}
