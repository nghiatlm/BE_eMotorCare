using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.PartTypeRepository
{
    public class PartTypeRepository : GenericRepository<PartType>, IPartTypeRepository
    {
        public PartTypeRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<(IReadOnlyList<PartType> Items, long Total)> GetPagedAsync(
            string? name,
            string? description,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.PartTypes
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                q = q.Where(x =>
                    x.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                q = q.Where(x =>
                    x.Description.Contains(description));
            }
            


            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<PartType?> GetByIdAsync(Guid id) =>
            _context.PartTypes.FirstOrDefaultAsync(x => x.Id == id);

    }
}
