using AutoMapper;
using eMotoCare.BLL.HashPassword;
using eMotoCare.Common.Enums;
using eMotoCare.Common.Exceptions;
using eMotoCare.Common.Models.Pages;
using eMotoCare.Common.Models.Responses;
using eMotoCare.DAL;
using eMotoCare.DAL.Entities;

namespace eMotoCare.BLL.Services.AdminServices
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _hasher;

        public AdminUserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher hasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hasher = hasher;
        }

        public async Task<PageResult<UserResponse>> GetPagedAsync(
            string? search,
            RoleName? role,
            AccountStatus? status,
            int page,
            int pageSize
        )
        {
            var (items, total) = await _unitOfWork.Accounts.GetPagedAsync(
                search,
                role,
                status,
                page,
                pageSize
            );
            var mapped = _mapper.Map<List<UserResponse>>(items);
            return new PageResult<UserResponse>(mapped, pageSize, page, (int)total);
        }

        public async Task<UserResponse?> GetByIdAsync(Guid id)
        {
            var acc = await _unitOfWork.Accounts.GetByIdAsync(id);
            return acc is null ? null : _mapper.Map<UserResponse>(acc);
        }

        public async Task<Guid> CreateAsync(
            string phone,
            string password,
            string fullName,
            RoleName role,
            string? email,
            string? avatarUrl
        )
        {
            if (
                !string.IsNullOrWhiteSpace(email)
                && await _unitOfWork.Accounts.ExistsEmailAsync(email)
            )
                throw new AppException(ErrorCode.EMAIL_ALREADY_EXISTS);

            var account = new Account
            {
                Phone = phone,
                Email = email,
                Role = role,
                AccountStatus = AccountStatus.ACTIVE, // admin tạo là active
                Password = _hasher.HashPassword(password),
                CreatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.Accounts.CreateAsync(account);
            await _unitOfWork.SaveChangesWithTransactionAsync();
            return account.AccountId;
        }

        public async Task<bool> UpdateAsync(
            Guid id,
            string? fullName,
            RoleName? role,
            string? email,
            AccountStatus? status
        )
        {
            if (id == Guid.Empty)
                throw new AppException(ErrorCode.NOT_FOUND);
            var acc =
                await _unitOfWork.Accounts.GetByIdAsync(id)
                ?? throw new AppException(ErrorCode.NOT_FOUND);

            if (
                !string.IsNullOrWhiteSpace(email)
                && email != acc.Email
                && await _unitOfWork.Accounts.ExistsEmailAsync(email)
            )
                throw new AppException(ErrorCode.EMAIL_ALREADY_EXISTS);

            if (role.HasValue)
                acc.Role = role.Value;
            if (!string.IsNullOrWhiteSpace(email))
                acc.Email = email;
            if (status.HasValue)
                acc.AccountStatus = status.Value;

            await _unitOfWork.Accounts.UpdateAsync(acc);
            await _unitOfWork.SaveChangesWithTransactionAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var acc =
                await _unitOfWork.Accounts.GetByIdAsync(id)
                ?? throw new AppException(ErrorCode.NOT_FOUND);
            _unitOfWork.Accounts.Delete(acc);
            await _unitOfWork.SaveChangesWithTransactionAsync();
            return true;
        }
    }
}
