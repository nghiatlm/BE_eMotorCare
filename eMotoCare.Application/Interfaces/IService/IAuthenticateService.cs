

using eMotoCare.Application.DTOs;

namespace eMotoCare.Application.Interfaces.IService
{
    public interface IAuthenticateService
    {
        public Task<bool> Register(RegisterRequest request);
    }
}
