using eMotoCare.Common.Enums;
using eMotoCare.Common.Models.Pages;
using eMotoCare.Common.Models.Responses;

namespace eMotoCare.BLL.Services.AdminServices
{
    public interface IAdminUserService
    {
        Task<PageResult<UserResponse>> GetPagedAsync(
            string? search,
            RoleName? role,
            AccountStatus? status,
            int page,
            int pageSize
        );
        Task<UserResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(
            string phone,
            string password,
            string fullName,
            RoleName role,
            string? email
        );
        Task<bool> UpdateAsync(
            Guid id,
            string? fullName,
            RoleName? role,
            string? email,
            AccountStatus? status
        );
        Task<bool> DeleteAsync(Guid id);
    }
}
