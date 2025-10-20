using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.StaffServices
{
    public interface IStaffService
    {
        Task<PageResult<StaffResponse>> GetPagedAsync(
            string? search,
            PositionEnum? position,
            Guid? serviceCenterId,
            int page,
            int pageSize
        );
        Task<StaffResponse?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(StaffRequest req);
        Task UpdateAsync(Guid id, StaffRequest req);
        Task DeleteAsync(Guid id);
    }
}
