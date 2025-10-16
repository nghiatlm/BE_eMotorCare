
using System.Net;
using eMotoCare.BO.DTO.ApiResponse;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL.Repositories.AccountRepository;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IAccountRepository _repository;

        public AuthService(ILogger<AuthService> logger, IAccountRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<AuthResponse> Login(LoginRequest request)
        {
            try
            {
                var account = await _repository.FindByPhone(request.Phone);
                if (account == null) throw new AppException("Tài khoảng không tồn tại", HttpStatusCode.NotFound);
                if (!account.Equals(request.Password)) throw new AppException("Mật khẩu không đúng", HttpStatusCode.BadRequest);
                return new AuthResponse
                {
                    Token = "",
                    AccountResponse = null,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: ${ex.Message}");
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public Task<AuthResponse> Register(RegisterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}