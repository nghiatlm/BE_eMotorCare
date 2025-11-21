

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.RMAServices
{
    public interface IRMAService
    {
        Task<Guid> CreateAsync(RMARequest req);
        Task DeleteAsync(Guid id);
        Task<List<RMAResponse?>> GetByCustomerIdAsync(Guid customerId);
        Task<RMAResponse?> GetByIdAsync(Guid id);
        Task<PageResult<RMAResponse>> GetPagedAsync(string? code, DateTime? fromDate, DateTime? toDate, string? returnAddress, RMAStatus? status, Guid? createdById, Guid? serviceCenterId, int page, int pageSize);
        Task UpdateAsync(Guid id, RMAUpdateRequest req);
    }
}
