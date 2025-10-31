using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ServiceCenterSlotRepository
{
    public interface IServiceCenterSlotRepository : IGenericRepository<ServiceCenterSlot>
    {
        Task<List<ServiceCenterSlot>> GetByServiceCenterAsync(Guid serviceCenterId);
        Task<bool> HasOverlapAsync(
            Guid serviceCenterId,
            DayOfWeeks dayOfWeek,
            TimeSpan start,
            TimeSpan end,
            Guid? excludeId = null
        );
        Task<List<ServiceCenterSlot>> GetByServiceCenterOnDateAsync(Guid scId, DateOnly date);
        Task<bool> HasOverlapOnDateAsync(
            Guid scId,
            DateOnly date,
            TimeSpan start,
            TimeSpan end,
            Guid? excludeId = null
        );
        Task<int> CountBookingsAsync(Guid serviceCenterId, Guid slotId, DateOnly date);
    }
}
