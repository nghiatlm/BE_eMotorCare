

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;

namespace eMototCare.BLL.Services.AuthServices
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginRequest request);
        Task<AuthResponse> Register(RegisterRequest request);
    }
}