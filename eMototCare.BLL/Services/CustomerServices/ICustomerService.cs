

using eMotoCare.BO.DTO.Requests;

namespace eMototCare.BLL.Services.CustomerServices
{
    public interface ICustomerService
    {
        public Task<Guid> CreateAsync(CustomerRequest req);
    }
}
