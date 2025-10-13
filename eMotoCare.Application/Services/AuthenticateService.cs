

using AutoMapper;
using eMotoCare.Application.DTOs;
using eMotoCare.Application.Exceptions;
using eMotoCare.Application.Interfaces;
using eMotoCare.Application.Interfaces.IService;
using eMotoCare.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Numerics;

namespace eMotoCare.Application.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticateService> _logger;
        private readonly IOtpService _otpService;

        public AuthenticateService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper, ILogger<AuthenticateService> logger, IOtpService otpService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _logger = logger;
            _otpService = otpService;
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
    }
}
