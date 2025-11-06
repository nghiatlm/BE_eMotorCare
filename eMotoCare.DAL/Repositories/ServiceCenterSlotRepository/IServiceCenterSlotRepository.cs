using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ServiceCenterSlotRepository
{
    public interface IServiceCenterSlotRepository : IGenericRepository<ServiceCenterSlot>
    {
        Task<List<ServiceCenterSlot>> GetByServiceCenterAsync(Guid serviceCenterId);

        Task<List<ServiceCenterSlot>> GetByServiceCenterOnDateAsync(Guid scId, DateOnly date);
        Task<bool> ExistsSlotAsync(Guid scId, DateOnly date, DayOfWeeks dow, SlotTime slot);
        Task<int> CountBookingsAsync(Guid serviceCenterId, DateOnly date, SlotTime slot);
    }
}
