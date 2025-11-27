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
using eMototCare.BLL.Services.EmailServices;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace eMototCare.BLL.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly IMemoryCache _cache;
        private readonly IEmailService _mailService;

        public AccountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AccountService> logger,
            IMemoryCache cache,
            IEmailService mailService
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _mailService = mailService;
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
                if (role.HasValue && role.Value == RoleName.ROLE_ADMIN)
                    throw new AppException(
                        "Không được tìm kiếm theo ROLE_ADMIN",
                        HttpStatusCode.Forbidden
                    );

                var (items, total) = await _unitOfWork.Accounts.GetPagedAsync(
                    search,
                    role,
                    status,
                    page,
                    pageSize
                );
                if (page <= 0)
                    throw new AppException("Page phải > 0", HttpStatusCode.BadRequest);

                if (pageSize <= 0)
                    throw new AppException("PageSize phải > 0", HttpStatusCode.BadRequest);

                if (role.HasValue && !Enum.IsDefined(typeof(RoleName), role.Value))
                    throw new AppException("Role không hợp lệ", HttpStatusCode.BadRequest);

                if (status.HasValue && !Enum.IsDefined(typeof(AccountStatus), status.Value))
                    throw new AppException(
                        "Trạng thái tài khoản không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
                var accountIds = items.Select(x => x.Id).ToList();
                var staffs = await _unitOfWork.Staffs.GetByAccountIdsAsync(accountIds);
                var staffByAcc = staffs.ToDictionary(s => s.AccountId, s => s);

                var customers = await _unitOfWork.Customers.GetByAccountIdsAsync(accountIds);
                var customerByAcc = customers.ToDictionary(c => c.AccountId, c => c);

                var rows = new List<AccountResponse>(items.Count);
                foreach (var acc in items)
                {
                    var dto = _mapper.Map<AccountResponse>(acc);

                    switch (acc.RoleName)
                    {
                        case RoleName.ROLE_TECHNICIAN:
                        case RoleName.ROLE_MANAGER:
                        case RoleName.ROLE_STAFF:
                            if (staffByAcc.TryGetValue(acc.Id, out var st))
                                dto.Staff = _mapper.Map<StaffResponse>(st);
                            break;

                        case RoleName.ROLE_CUSTOMER:
                            if (customerByAcc.TryGetValue(acc.Id, out var cu))
                                dto.Customer = _mapper.Map<CustomerResponse>(cu);
                            break;
                    }

                    rows.Add(dto);
                }

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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
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
                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);
                var phone = (req.Phone ?? string.Empty).Trim();
                var email = req.Email?.Trim().ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
                    throw new AppException(
                        "Cần ít nhất Phone hoặc Email",
                        HttpStatusCode.BadRequest
                    );
                if (!Enum.IsDefined(typeof(RoleName), req.RoleName))
                    throw new AppException("Role không hợp lệ", HttpStatusCode.BadRequest);
                if (!Enum.IsDefined(typeof(AccountStatus), req.Status))
                    throw new AppException(
                        "Trạng thái tài khoản không hợp lệ",
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
                if (
                    req.RoleName == RoleName.ROLE_STAFF
                    || req.RoleName == RoleName.ROLE_MANAGER
                    || req.RoleName == RoleName.ROLE_TECHNICIAN
                    || req.RoleName == RoleName.ROLE_STOREKEEPER
                )
                {
                    var otp = new Random().Next(100000, 999999).ToString();
                    _cache.Set($"OTP_{email}", otp, TimeSpan.FromMinutes(5));
                    await _mailService.SendLoginEmailAsync(
                        email,
                        "Xác minh đăng nhập nhân viên",
                        otp
                    );
                }
                var entity = _mapper.Map<Account>(req);
                entity.Id = Guid.NewGuid();
                entity.Phone = phone;
                entity.Email = email;

                entity.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);

                entity.RoleName = req.RoleName;
                entity.Stattus = AccountStatus.IN_ACTIVE;

                await _unitOfWork.Accounts.CreateAsync(entity);
                if (req.Staff != null)
                {
                    var staffEntity = _mapper.Map<Staff>(req.Staff);
                    staffEntity.Id = Guid.NewGuid();
                    staffEntity.AccountId = entity.Id;
                    staffEntity.CreatedAt = DateTime.UtcNow;

                    await _unitOfWork.Staffs.CreateAsync(staffEntity);
                }
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);

                if (req == null)
                    throw new AppException("Request không được null", HttpStatusCode.BadRequest);
                var entity =
                    await _unitOfWork.Accounts.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy tài khoản", HttpStatusCode.NotFound);

                var newPhone = (req.Phone ?? string.Empty).Trim();
                var newEmail = req.Email?.Trim().ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(newPhone) && string.IsNullOrWhiteSpace(newEmail))
                    throw new AppException(
                        "Không được xoá cả Phone và Email",
                        HttpStatusCode.BadRequest
                    );
                if (!Enum.IsDefined(typeof(RoleName), req.RoleName))
                    throw new AppException("Role không hợp lệ", HttpStatusCode.BadRequest);
                if (!Enum.IsDefined(typeof(AccountStatus), req.Status))
                    throw new AppException(
                        "Trạng thái tài khoản không hợp lệ",
                        HttpStatusCode.BadRequest
                    );
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
                if (req.Staff != null)
                {
                    if (
                        req.Staff.ServiceCenterId.HasValue
                        && req.Staff.ServiceCenterId.Value != Guid.Empty
                    )
                    {
                        var scId = req.Staff.ServiceCenterId.Value;
                        var scExists = await _unitOfWork.ServiceCenters.ExistsAsync(scId);
                        if (!scExists)
                            throw new AppException(
                                "Service Center không tồn tại",
                                HttpStatusCode.BadRequest
                            );
                    }

                    var existingStaff = await _unitOfWork.Staffs.GetByAccountIdAsync(entity.Id);

                    if (existingStaff != null)
                    {
                        _mapper.Map(req.Staff, existingStaff);
                        existingStaff.AccountId = entity.Id;
                        existingStaff.UpdatedAt = DateTime.UtcNow;
                        await _unitOfWork.Staffs.UpdateAsync(existingStaff);
                    }
                    else
                    {
                        var newStaff = _mapper.Map<Staff>(req.Staff);
                        newStaff.Id = Guid.NewGuid();
                        newStaff.AccountId = entity.Id;
                        newStaff.CreatedAt = DateTime.UtcNow;

                        await _unitOfWork.Staffs.CreateAsync(newStaff);
                    }
                }
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
                if (id == Guid.Empty)
                    throw new AppException("Id không hợp lệ", HttpStatusCode.BadRequest);
                var entity =
                    await _unitOfWork.Accounts.GetByIdAsync(id)
                    ?? throw new AppException("Không tìm thấy tài khoản", HttpStatusCode.NotFound);
                entity.Stattus = AccountStatus.IN_ACTIVE;
                await _unitOfWork.Accounts.DeleteAsync(entity);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation("Admin deactivated account {Id}", id);
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deactivate admin user failed: {Message}", ex.Message);
                throw new AppException("Internal Server Error", HttpStatusCode.InternalServerError);
            }
        }
    }
}
