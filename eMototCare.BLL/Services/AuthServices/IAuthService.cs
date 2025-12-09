

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;

namespace eMototCare.BLL.Services.AuthServices
{
    public interface IAuthService
    {
        Task ActiveAccount(string phone);
        Task<bool> ChangePassword(string oldPassword, string newPassword, Guid id);
        Task<AuthResponse> Login(LoginRequest request);
        Task<AuthResponse?> LoginStaff(StaffLoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<bool> VerifyEmailAsync(string token);
    }
}