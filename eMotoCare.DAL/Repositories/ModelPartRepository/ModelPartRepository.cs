using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ModelPartTypeRepository
{
    public class ModelPartRepository : GenericRepository<ModelPart>, IModelPartRepository
    {
        public ModelPartRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<ModelPart?> GetByIdAsync(Guid id) =>
            await _context
                .ModelParts.Include(x => x.Model)
                .Include(x => x.Part)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> ExistsAsync(Guid modelId, Guid partId, Guid? ignoreId = null)
        {
            var q = _context.ModelParts.Where(x => x.ModelId == modelId && x.PartId == partId);

            if (ignoreId.HasValue)
                q = q.Where(x => x.Id != ignoreId.Value);

            return await q.AnyAsync();
        }

        public async Task<(IReadOnlyList<ModelPart> Items, long Total)> GetPagedAsync(
            string? search,
            Guid? modelId,
            Guid? partId,
            Status? status,
            Guid? id,
            int page,
            int pageSize
        )
        {
            var q = _context.ModelParts.Include(x => x.Model).Include(x => x.Part).AsQueryable();

            if (id.HasValue)
                q = q.Where(x => x.Id == id.Value);

            if (modelId.HasValue)
                q = q.Where(x => x.ModelId == modelId.Value);

            if (partId.HasValue)
                q = q.Where(x => x.PartId == partId.Value);

            if (status.HasValue)
                q = q.Where(x => x.Status == status.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var text = search.Trim().ToLower();
                q = q.Where(x =>
                    x.Model!.Name.ToLower().Contains(text) || x.Part!.Name.ToLower().Contains(text)
                );
            }

            var total = await q.LongCountAsync();

            var items = await q
            //.OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
