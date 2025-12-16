using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.DAL;
using eMotoCare.DAL.Repositories.AccountRepository;
using eMototCare.BLL.HashPasswords;
using eMototCare.BLL.JwtServices;
using eMototCare.BLL.Services.EmailServices;
using FirebaseAdmin;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Security.Claims;

namespace eMototCare.BLL.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _mailService;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger,
            IAccountRepository repository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IEmailService mailService,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _mailService = mailService;
            _configuration = configuration;
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
                account.LoginCount += 1;
                await _unitOfWork.Accounts.UpdateAsync(account);
                await _unitOfWork.SaveAsync();
                return new AuthResponse
                {
                    Token = token,
                    AccountResponse = new AccountResponse
                    {
                        Id = account.Id,
                        Email = account.Email,
                        Phone = account.Phone,
                        RoleName = account.RoleName,
                        Status = account.Stattus,
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

        public async Task<AuthResponse?> LoginStaff(StaffLoginRequest request)
        {
            try
            {
                var account = await _unitOfWork.Accounts.FindByEmail(request.Email);
                if (account == null)
                    throw new AppException("Tài khoản không tồn tại", HttpStatusCode.NotFound);

                if (account.Password == _configuration["DefaultPassword:password"] && account.LoginCount == 0)
                {
                    if (account.Stattus == AccountStatus.IN_ACTIVE)
                    {
                        string verifyToken = _jwtService.GenerateEmailVerificationToken(account.Email);
                        string url = $"https://bemodernestate.site/api/v1/auths/verify/staff?token={verifyToken}";
                        //string url = $"https://localhost:7134/api/v1/auths/verify/staff?token={verifyToken}";
                        await _mailService.SendLoginEmailAsync(
                            account.Email,
                            "Xác minh tài khoản nhân viên",
                            url
                        );
                        return null;
                    }
                    string token_ = _jwtService.GenerateJwtToken(account);
                    account.LoginCount += 1;
                    await _unitOfWork.Accounts.UpdateAsync(account);
                    await _unitOfWork.SaveAsync();
                    return new AuthResponse
                    {
                        Token = token_,
                        AccountResponse = new AccountResponse
                        {
                            Id = account.Id,
                            Email = account.Email,
                            Phone = account.Phone,
                            RoleName = account.RoleName,
                            Status = account.Stattus,
                        },
                    };
                }

                if (account.Password == _configuration["DefaultPassword:password"] && account.LoginCount == 1)
                {
                    account.Stattus = AccountStatus.IN_ACTIVE;
                    await _unitOfWork.Accounts.UpdateAsync(account);
                    await _unitOfWork.SaveAsync();
                    throw new AppException("Tài khoản đã bị khoá, vui lòng liên hệ ADMIN để mở khoá", HttpStatusCode.Locked);
                }

                bool checkPasswo5rd = _passwordHasher.VerifyPassword(
                    request.Password,
                    account.Password
                );
                if (!checkPasswo5rd)
                    throw new AppException("Mật khẩu không đúng", HttpStatusCode.BadRequest);
                if (
                    account.RoleName != RoleName.ROLE_STAFF
                    && account.RoleName != RoleName.ROLE_MANAGER
                    && account.RoleName != RoleName.ROLE_ADMIN
                    && account.RoleName != RoleName.ROLE_STOREKEEPER
                    && account.RoleName != RoleName.ROLE_TECHNICIAN
                )
                {
                    throw new AppException(
                        "Tài khoản không phải nhân viên",
                        HttpStatusCode.Forbidden
                    );
                }
                if (account.Stattus == AccountStatus.IN_ACTIVE)
                {
                    string verifyToken = _jwtService.GenerateEmailVerificationToken(account.Email);
                    string url = $"https://bemodernestate.site/api/v1/auths/verify/staff?token={verifyToken}";
                    //string url = $"https://localhost:7134/api/v1/auths/verify/staff?token={verifyToken}";
                    await _mailService.SendLoginEmailAsync(
                        account.Email,
                        "Xác minh tài khoản nhân viên",
                        url
                    );
                    return null;
                }
                account.LoginCount += 1;
                await _unitOfWork.Accounts.UpdateAsync(account);
                await _unitOfWork.SaveAsync();
                string token = _jwtService.GenerateJwtToken(account);
                return new AuthResponse
                {
                    Token = token,
                    AccountResponse = new AccountResponse
                    {
                        Id = account.Id,
                        Email = account.Email,
                        Phone = account.Phone,
                        RoleName = account.RoleName,
                        Status = account.Stattus,
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
                var phone = request.Phone.Trim();
                if (await _unitOfWork.Accounts.ExistsPhoneAsync(phone))
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);

                var account = await _unitOfWork.Accounts.CreateAsync(
                    new Account
                    {
                        Phone = phone,
                        Password = _passwordHasher.HashPassword(request.Password),
                        RoleName = RoleName.ROLE_CUSTOMER,
                        Stattus = AccountStatus.IN_ACTIVE,
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

        public async Task ActiveAccount(string phone)
        {
            var account = await _unitOfWork.Accounts.GetByPhoneAsync(phone);
            account.Stattus = AccountStatus.ACTIVE;
            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            try
            {
                var principal = _jwtService.ValidateTokenClaimsPrincipal(token);
                var email = principal.FindFirst(ClaimTypes.Email)?.Value;

                if (email == null)
                    throw new AppException("Email không tồn tại");

                var account = await _unitOfWork.Accounts.GetByEmailAsync(email);
                if (account == null)
                    throw new AppException("Không tìm thấy tài khoản");

                if (account.Stattus == AccountStatus.ACTIVE)
                    throw new AppException("Tài khoản đã kích hoạt rồi");

                account.Stattus = AccountStatus.ACTIVE;
                await _unitOfWork.Accounts.UpdateAsync(account);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (SecurityTokenExpiredException e)
            {
                throw new AppException(e.Message);
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword, Guid id)
        {
            try
            {

                var account = await _unitOfWork.Accounts.GetByIdAsync(id);
                if (account == null) throw new AppException("Không tìm thấy account", HttpStatusCode.NotFound);

                if (account.Password == _configuration["DefaultPassword:password"])
                {
                    if (oldPassword == account.Password)
                    {
                        account.Password = _passwordHasher.HashPassword(newPassword);
                        await _unitOfWork.Accounts.UpdateAsync(account);
                        await _unitOfWork.SaveAsync();
                        return true;
                    } else
                    {
                        throw new AppException("Mật khẩu không trùng", HttpStatusCode.BadRequest);
                    }
                }

                bool checkPassword = _passwordHasher.VerifyPassword(oldPassword, account.Password);
                if (!checkPassword) throw new AppException("Mật khẩu không trùng", HttpStatusCode.BadRequest);
                account.Password = _passwordHasher.HashPassword(newPassword);
                await _unitOfWork.Accounts.UpdateAsync(account);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ex.Message);
            }
        }
    }
}
