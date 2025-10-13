

using AutoMapper;
using eMotoCare.Application.DTOs;
using eMotoCare.Application.Exceptions;
using eMotoCare.Application.Interfaces;
using eMotoCare.Application.Interfaces.IService;
using eMotoCare.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;


namespace eMotoCare.Application.Services
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
                account.Role = Domain.Enums.RoleName.ROLE_CUSTOMER;
                account.AccountStatus = Domain.Enums.AccountStatus.WAITING_FOR_CONFIRMATION;
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
            var cacheKey = $"otp_{phone}";
            if (_cache.TryGetValue(cacheKey, out string? storedCode))
            {
                if (storedCode == code)
                {
                    _cache.Remove(cacheKey); // Xóa sau khi dùng
                    return true;
                }
            }
            var account = await _unitOfWork.Accounts.GetByPhoneAsync(phone);
            if (account == null)
                throw new AppException(ErrorCode.PHONE_DO_NOT_EXISTS);
            account.AccountStatus = Domain.Enums.AccountStatus.ACTIVE;
            _unitOfWork.Accounts.Update(account);
            await _unitOfWork.SaveChangesWithTransactionAsync();
            return false;
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

    }
}
