using System.Net;
using AutoMapper;
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Exceptions;
using eMotoCare.BO.Pages;
using eMotoCare.DAL;
using eMototCare.BLL.Services.AccountService;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AccountService> logger
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageResult<AccountResponse>> GetPagedAsync(
            string? search,
            RoleName? role,
            AccountStatus? status,
            int page,
            int pageSize
        )
        {
            try
            {
                var (items, total) = await _unitOfWork.Accounts.GetPagedAsync(
                    search,
                    role,
                    status,
                    page,
                    pageSize
                );
                var rows = _mapper.Map<List<AccountResponse>>(items);
                return new PageResult<AccountResponse>(rows, pageSize, page, (int)total);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetPaged Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<AccountResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var acc =
                    await _unitOfWork.Accounts.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy tài khoản", HttpStatusCode.NotFound);
                return _mapper.Map<AccountResponse>(acc);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetById Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<Guid> CreateAsync(AccountRequest req)
        {
            try
            {
                var phone = (req.Phone ?? string.Empty).Trim();
                var email = req.Email?.Trim().ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
                    throw new AppException(
                        "Cần ít nhất Phone hoặc Email",
                        HttpStatusCode.BadRequest
                    );

                if (
                    !string.IsNullOrWhiteSpace(phone)
                    && await _unitOfWork.Accounts.ExistsPhoneAsync(phone)
                )
                    throw new AppException("Số điện thoại đã tồn tại", HttpStatusCode.Conflict);

                if (
                    !string.IsNullOrWhiteSpace(email)
                    && await _unitOfWork.Accounts.ExistsEmailAsync(email!)
                )
                    throw new AppException("Email đã tồn tại", HttpStatusCode.Conflict);

                if (string.IsNullOrWhiteSpace(req.Password))
                    throw new AppException(
                        "Mật khẩu không được để trống",
                        HttpStatusCode.BadRequest
                    );

                var entity = _mapper.Map<Account>(req);
                entity.Id = Guid.NewGuid();
                entity.Phone = phone;
                entity.Email = email;

                entity.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);

                entity.RoleName = req.RoleName;
                entity.Stattus = req.Status;

                await _unitOfWork.Accounts.CreateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation(
                    "Created Account {Id} ({Phone}/{Email})",
                    entity.Id,
                    entity.Phone,
                    entity.Email
                );
                return entity.Id;
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task UpdateAsync(Guid id, AccountRequest req)
        {
            try
            {
                var entity =
                    await _unitOfWork.Accounts.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy tài khoản", HttpStatusCode.NotFound);

                var newPhone = (req.Phone ?? string.Empty).Trim();
                var newEmail = req.Email?.Trim().ToLowerInvariant();

                if (
                    !string.IsNullOrWhiteSpace(newPhone)
                    && !string.Equals(entity.Phone, newPhone, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Accounts.ExistsPhoneAsync(newPhone)
                )
                    throw new AppException("Số điện thoại đã tồn tại", HttpStatusCode.Conflict);

                if (
                    !string.IsNullOrWhiteSpace(newEmail)
                    && !string.Equals(entity.Email, newEmail, StringComparison.OrdinalIgnoreCase)
                    && await _unitOfWork.Accounts.ExistsEmailAsync(newEmail!)
                )
                    throw new AppException("Email đã tồn tại", HttpStatusCode.Conflict);

                _mapper.Map(req, entity);

                entity.Phone = newPhone;
                entity.Email = newEmail;
                entity.RoleName = req.RoleName;
                entity.Stattus = req.Status;

                if (!string.IsNullOrWhiteSpace(req.Password))
                {
                    entity.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);
                }

                await _unitOfWork.Accounts.UpdateAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Updated Account {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity =
                    await _unitOfWork.Accounts.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy tài khoản", HttpStatusCode.NotFound);

                await _unitOfWork.Accounts.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Deleted Account {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Account failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
