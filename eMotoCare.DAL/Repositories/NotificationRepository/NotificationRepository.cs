

using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using eMotoCare.DAL.Repositories.ModelRepository;

namespace eMotoCare.DAL.Repositories.NotificationRepository
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context)
            : base(context) { }
    }
}
