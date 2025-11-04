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
using eMototCare.BLL.Services.EmailServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _mailService;

        public AuthService(
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger,
            IAccountRepository repository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IMemoryCache cache,
            IEmailService mailService
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _cache = cache;
            _mailService = mailService;
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

        public async Task<bool> LoginStaff(StaffLoginRequest request)
        {
            try
            {
                var account = await _unitOfWork.Accounts.FindByEmail(request.Email);
                if (account == null)
                    throw new AppException("Tài khoản không tồn tại", HttpStatusCode.NotFound);
                bool checkPasswo5rd = _passwordHasher.VerifyPassword(
                    request.Password,
                    account.Password
                );
                if (!checkPasswo5rd)
                    throw new AppException("Mật khẩu không đúng", HttpStatusCode.BadRequest);
                if (account.RoleName != RoleName.ROLE_STAFF &&
                    account.RoleName != RoleName.ROLE_MANAGER &&
                    account.RoleName != RoleName.ROLE_ADMIN &&
                    account.RoleName != RoleName.ROLE_STOREKEEPER)
                {
                    throw new AppException("Tài khoản không phải nhân viên", HttpStatusCode.Forbidden);
                }
                if (account.Stattus != AccountStatus.ACTIVE)
                    throw new AppException("Tài khoản chưa được kích hoạt", HttpStatusCode.Forbidden);

                var otp = new Random().Next(100000, 999999).ToString();

                _cache.Set($"OTP_{account.Email}", otp, TimeSpan.FromMinutes(5));


                await _mailService.SendLoginEmailAsync(account.Email, "Xác minh đăng nhập nhân viên", otp);

                return true;
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

        public async Task<AuthResponse> VerifyLoginStaffAsync(VerifyLoginRequest request)
        {
            var account = await _unitOfWork.Accounts.FindByEmail(request.Email);
            if (account == null)
                throw new AppException("Tài khoản không tồn tại", HttpStatusCode.NotFound);

            if (!_cache.TryGetValue($"OTP_{request.Email}", out string cachedOtp))
                throw new AppException("OTP đã hết hạn hoặc không tồn tại", HttpStatusCode.BadRequest);

            if (cachedOtp != request.Otp)
                throw new AppException("OTP không đúng", HttpStatusCode.BadRequest);

            _cache.Remove($"OTP_{request.Email}");

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
                    Stattus = account.Stattus
                }
            };
        }


        public async Task<bool> Register(RegisterRequest request)
        {
            try
            {
                var phone = request.Phone.Trim();
                if (await _unitOfWork.Accounts.ExistsPhoneAsync(phone))
                    throw new AppException("Code đã tồn tại", HttpStatusCode.Conflict);

                var account = await _unitOfWork.Accounts.CreateAsync(new Account
                {
                    Phone = phone,
                    Password = _passwordHasher.HashPassword(request.Password),
                    RoleName = RoleName.ROLE_CUSTOMER,
                    Stattus = AccountStatus.IN_ACTVIE,
                });
                if (account < 1) throw new AppException("Tạo không thành công", HttpStatusCode.BadRequest);
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
    }
}
