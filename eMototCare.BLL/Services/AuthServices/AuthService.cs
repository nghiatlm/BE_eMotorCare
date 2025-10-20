using System.Net;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using eMotoCare.DAL.Repositories.AccountRepository;
using eMototCare.BLL.HashPasswords;
using eMototCare.BLL.JwtServices;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger,
            IAccountRepository repository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<AuthResponse> Login(LoginRequest request)
        {
            try
            {
                var account = await _unitOfWork.Accounts.FindByPhone(request.Phone);
                if (account == null)
                    throw new AppException("Tài khoản không tồn tại", HttpStatusCode.NotFound);
                bool checkPasswo5rd = _passwordHasher.VerifyPassword(
                    request.Password,
                    account.Password
                );
                if (!checkPasswo5rd)
                    throw new AppException("Mật khẩu không đúng", HttpStatusCode.BadRequest);
                string token = _jwtService.GenerateJwtToken(account);
                if (token == null)
                    throw new AppException("Token không được null", HttpStatusCode.BadRequest);
                return new AuthResponse
                {
                    Token = token,
                    AccountResponse = new AccountResponse
                    {
                        Id = account.Id,
                        Email = account.Email,
                        Phone = account.Phone,
                        RoleName = account.RoleName,
                        Stattus = account.Stattus,
                    },
                };
            }
            catch (AppException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: ${ex.Message}");
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            try
            {
                var account = await _unitOfWork.Accounts.CreateAsync(
                    new Account
                    {
                        Phone = request.Phone,
                        Password = _passwordHasher.HashPassword(request.Password),
                        RoleName = RoleName.ROLE_CUSTOMER,
                        Stattus = AccountStatus.ACTIVE,
                    }
                );
                if (account < 1)
                    throw new AppException("Tạo không thành công", HttpStatusCode.BadRequest);
                var result = await _unitOfWork.SaveAsync();
                return result > 0 ? true : false;
            }
            catch (AppException aex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: ${ex.Message}");
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
