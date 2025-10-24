using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ServiceCenterSlotRepository
{
    public interface IServiceCenterSlotRepository : IGenericRepository<ServiceCenterSlot>
    {
        Task<List<ServiceCenterSlot>> GetByServiceCenterAsync(Guid serviceCenterId);
        Task<bool> HasOverlapAsync(
            Guid serviceCenterId,
            DayOfWeek dayOfWeek,
            TimeSpan start,
            TimeSpan end,
            Guid? excludeId = null
        );

        Task<int> CountBookingsAsync(Guid serviceCenterId, Guid slotId, DateOnly date);
    }
}
