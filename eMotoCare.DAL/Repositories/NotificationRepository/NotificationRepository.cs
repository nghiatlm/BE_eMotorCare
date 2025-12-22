

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
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

        public async Task<(IReadOnlyList<Notification> Items, long Total)> GetPagedAsync(
            Guid? receiverId,
            NotificationEnum? notificationType,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.Notifications
                .Include(x => x.Receiver)
                .ThenInclude(x => x.Customer)
                .AsQueryable();

            if (receiverId.HasValue)
            {
                q = q.Where(x =>
                    x.ReceiverId  == receiverId);
            }

            if (notificationType.HasValue)
            {
                q = q.Where(x =>
                    x.Type == notificationType);
            }



            var total = await q.LongCountAsync();

            var items = await q
                .OrderBy(x => x.SentAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
