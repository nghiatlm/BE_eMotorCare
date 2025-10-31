

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;

namespace eMototCare.BLL.Services.AuthServices
{
    public interface IAuthService
    {
        Task ActiveAccount(string phone);
        Task<AuthResponse> Login(LoginRequest request);
        Task<bool> LoginStaff(StaffLoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<AuthResponse> VerifyLoginStaffAsync(VerifyLoginRequest request);
    }
}