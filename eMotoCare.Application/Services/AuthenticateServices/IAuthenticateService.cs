
using eMotoCare.Common.Models;

namespace eMotoCare.BLL.Services.AuthenticateService
{
    public interface IAuthenticateService
    {
        public Task<bool> Register(RegisterRequest request);
        public Task<bool> VerifyOtpAsync(string phone, string code);
        public Task<AuthenticateResponse> Login(string email, string password);

    }
}
