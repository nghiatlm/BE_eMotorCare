

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.NotificationRepository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<Notification?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<Notification> Items, long Total)> GetPagedAsync(Guid? receiverId, NotificationEnum? notificationType, int page, int pageSize);
    }
}
