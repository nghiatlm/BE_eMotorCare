

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.CustomerServices
{
    public interface ICustomerService
    {
        public Task<Guid> CreateAsync(CustomerRequest req);
        public Task DeleteAsync(Guid id);
        public Task<CustomerResponse?> GetByIdAsync(Guid id);
        public Task UpdateAsync(Guid id, CustomerRequest req);
        public Task<PageResult<CustomerResponse>> GetPagedAsync(
            string? firstName,
            string? lastName,
            string? address,
            string? citizenId,
            Guid? accountId,
            int page,
            int pageSize
        );
    }
}
