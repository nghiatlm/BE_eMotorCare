

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.NotificationServices
{
    public interface INotificationService
    {
        Task<Guid> CreateAsync(NotificationRequest req);
        Task<NotificationResponse?> GetByIdAsync(Guid id);
        Task<PageResult<NotificationResponse>> GetPagedAsync(Guid? receiverId, NotificationEnum? notificationType, int page, int pageSize);
    }
}
