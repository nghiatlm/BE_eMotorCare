using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<PageResult<CustomerResponse>> GetPagedAsync(string? search, int page, int pageSize);
        Task<CustomerResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(CustomerRequest req);
        Task UpdateAsync(Guid id, CustomerRequest req);
        Task DeleteAsync(Guid id);
        Task<CustomerResponse?> GetAccountIdAsync(Guid id);
    }
}
