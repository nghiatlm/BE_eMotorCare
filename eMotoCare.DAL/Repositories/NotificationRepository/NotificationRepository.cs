

using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.NotificationRepository
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context)
            : base(context) { }

        public Task<Notification?> GetByIdAsync(Guid id) =>
        _context.Notifications
            .Include(x => x.Receiver)
            .ThenInclude(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
