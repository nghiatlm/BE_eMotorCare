

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;

namespace eMototCare.BLL.Services.NotificationServices
{
    public interface INotificationService
    {
        Task<Guid> CreateAsync(NotificationRequest req);
        Task<NotificationResponse?> GetByIdAsync(Guid id);
    }
}
