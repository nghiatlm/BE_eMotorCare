using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.AccountService
{
    public interface IAccountService
    {
        Task<PageResult<AccountResponse>> GetPagedAsync(
            string? search,
            RoleName? role,
            AccountStatus? status,
            int page,
            int pageSize
        );

        Task<AccountResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(AccountRequest req);
        Task UpdateAsync(Guid id, AccountRequest req);
        Task DeleteAsync(Guid id);
    }
}
