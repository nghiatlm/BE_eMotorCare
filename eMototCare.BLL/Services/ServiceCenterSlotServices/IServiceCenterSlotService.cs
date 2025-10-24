using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;

namespace eMototCare.BLL.Services.ServiceCenterSlotServices
{
    public interface IServiceCenterSlotService
    {
        Task<List<ServiceCenterSlotResponse>> GetAllAsync(Guid serviceCenterId);
        Task<Guid> CreateAsync(Guid serviceCenterId, ServiceCenterSlotRequest req);
        Task UpdateAsync(Guid serviceCenterId, Guid slotId, ServiceCenterSlotRequest req);
        Task DeleteAsync(Guid serviceCenterId, Guid slotId);
        Task<List<(ServiceCenterSlotResponse Slot, int Remaining)>> GetAvailabilityAsync(
            Guid serviceCenterId,
            DateOnly date
        );
    }
}
