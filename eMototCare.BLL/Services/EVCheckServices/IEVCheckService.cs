

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.EVCheckServices
{
    public interface IEVCheckService
    {
        Task<Guid> CreateAsync(EVCheckRequest req);
        Task DeleteAsync(Guid id);
        Task<EVCheckResponse?> GetByIdAsync(Guid id);
        Task<PageResult<EVCheckResponse>> GetPagedAsync(DateTime? startDate, DateTime? endDate, EVCheckStatus? status, Guid? appointmentId, Guid? taskExecutorId, int page, int pageSize);
        Task UpdateAsync(Guid id, EVCheckRequest req);
    }
}
