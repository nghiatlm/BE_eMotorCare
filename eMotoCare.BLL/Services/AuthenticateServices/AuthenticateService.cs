

using AutoMapper;

using eMotoCare.BLL.HashPassword;
using eMotoCare.BLL.JwtServices;
using eMotoCare.BLL.Services.OtpServices;
using eMotoCare.Common.Enums;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.ApiResponse;
using eMotoCare.Common.Models.Requests;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL;
using eMotoCare.DAL.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;


namespace eMotoCare.BLL.Services.AuthenticateService
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticateService> _logger;
        private readonly IOtpService _otpService;
        private readonly IMemoryCache _cache;
        private readonly IJwtService _jwtService;

        public AuthenticateService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper, ILogger<AuthenticateService> logger, IOtpService otpService, IMemoryCache cache, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _logger = logger;
            _otpService = otpService;
            _cache = cache;
            _jwtService = jwtService;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            try
            {
                var existingAccount = await _unitOfWork.Accounts.GetByPhoneAsync(request.Phone);
                if (existingAccount != null)
                {
                    _logger.LogWarning("Không thể tạo tài khoản, số điện thoại {Email} đã tồn tại.", request.Phone);
                    throw new AppException(ErrorCode.EMAIL_ALREADY_EXISTS);
                }
                var account = _mapper.Map<Account>(request);
                account.Role = RoleName.ROLE_CUSTOMER;
                account.AccountStatus = AccountStatus.WAITING_FOR_CONFIRMATION;
                if (request.Password != request.ConfirmPassword) throw new AppException(ErrorCode.INVALID_PASSWORD);
                account.Password = _passwordHasher.HashPassword(request.Password);
                await _unitOfWork.Accounts.CreateAsync(account);
                await _unitOfWork.SaveChangesWithTransactionAsync();
                //Implement send OTP to phone number
                await _otpService.GenerateAndSendOtpAsync(request.Phone);

                _logger.LogInformation("Tạo tài khoản mới thành công với số điện thoại {Email}, Role: {Role}", request.Email, account.Role);
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
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> VerifyOtpAsync(string phone, string code)
        {
            var verify = await _otpService.VerifyOtpAsync(phone, code);
            if (!verify)
                throw new AppException(ErrorCode.INVALID_OTP);
            var account = await _unitOfWork.Accounts.GetByPhoneAsync(phone);
            if (account == null)
                throw new AppException(ErrorCode.PHONE_DO_NOT_EXISTS);
            account.AccountStatus = AccountStatus.ACTIVE;
            _unitOfWork.Accounts.Update(account);
            await _unitOfWork.SaveChangesWithTransactionAsync();
            return true;
        }

        public async Task<AuthenticateResponse> Login(string email, string password)
        {
            try
            {
                Account? account = await _unitOfWork.Accounts.GetByPhoneAsync(email);
                if (account == null) throw new AppException(ErrorCode.PHONE_DO_NOT_EXISTS);
                bool checkPassword = _passwordHasher.VerifyPassword(password, account.Password);
                if (!checkPassword) throw new AppException(ErrorCode.INVALID_PASSWORD);
                var token = _jwtService.GenerateJwtToken(account);
                if (string.IsNullOrEmpty(token)) throw new AppException(ErrorCode.TOKEN_NOT_NULL);
                var expiredAt = _jwtService.GetExpire(token);
                var authenticateResponse = new AuthenticateResponse
                {
                    Token = token,
                    ExpiredAt = expiredAt
                };
                return authenticateResponse;
            }
            catch (AppException ex)
            {
                _logger.LogWarning(ex, "AppException occurred: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword, string confirmPassword, Guid id)
        {
            try
            {
                if (string.IsNullOrEmpty(id.ToString())) throw new AppException(ErrorCode.UNAUTHORIZED);
                var account = await _unitOfWork.Accounts.GetByIdAsync(id);
                if (account == null) throw new AppException(ErrorCode.NOT_FOUND);
                bool checkPassword = _passwordHasher.VerifyPassword(oldPassword, account.Password);
                if (!checkPassword) throw new AppException(ErrorCode.INVALID_PASSWORD);
                if (newPassword != confirmPassword) throw new AppException(ErrorCode.INVALID_PASSWORD);
                account.Password = _passwordHasher.HashPassword(newPassword);
                await _unitOfWork.Accounts.UpdateAsync(account);
                await _unitOfWork.SaveChangesWithTransactionAsync();
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
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<ForgetPasswordResponse> ForgotPasswordAsync(string phone)
        {
            var account = await _unitOfWork.Accounts.GetByPhoneAsync(phone);
            if (account == null)
                return ForgetPasswordResponse.Fail("Phone number not found");

            await _otpService.GenerateAndSendOtpAsync(phone);
            return ForgetPasswordResponse.Ok("Password reset OTP has been sent to your phone number.");
        }

        public async Task<ForgetPasswordResponse> ResetPasswordAsync(string phoneNumber, string Otp, string newPassword, string confirmPassword)
        {
            var account = await _unitOfWork.Accounts.GetByPhoneAsync(phoneNumber);

            if (account == null) return ForgetPasswordResponse.Fail("Invalid phone number.");

            var verify = await _otpService.VerifyOtpAsync(phoneNumber, Otp);
            if (!verify)
                return ForgetPasswordResponse.Fail("Invalid or expired OTP.");
            if (newPassword != confirmPassword)
                return ForgetPasswordResponse.Fail("New password and confirm password do not match.");
            account.Password = _passwordHasher.HashPassword(newPassword);
            

            await _unitOfWork.Accounts.UpdateAsync(account);
            await _unitOfWork.SaveChangesWithTransactionAsync();

            return ForgetPasswordResponse.Ok("Password has been reset successfully.");
        }
    }
}
