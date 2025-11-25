using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ModelPartTypeRepository
{
    public class ModelPartTypeRepository
        : GenericRepository<ModelPartType>,
            IModelPartTypeRepository
    {
        public ModelPartTypeRepository(ApplicationDbContext context)
            : base(context) { }

        public async Task<ModelPartType?> GetByIdAsync(Guid id) =>
            await _context
                .ModelPartTypes.Include(m => m.Model)
                .Include(pt => pt.PartType)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> ExistsAsync(Guid modelId, Guid partTypeId, Guid? ignoreId = null)
        {
            var q = _context.ModelPartTypes.Where(x =>
                x.ModelId == modelId && x.PartTypeId == partTypeId
            );

            if (ignoreId.HasValue)
                q = q.Where(x => x.Id != ignoreId.Value);

            return await q.AnyAsync();
        }

        public async Task<(IReadOnlyList<ModelPartType> Items, long Total)> GetPagedAsync(
            string? search,
            Guid? id,
            Guid? modelId,
            Guid? partTypeId,
            Status? status,
            int page,
            int pageSize
        )
        {
            var q = _context
                .ModelPartTypes.AsNoTracking()
                .Include(x => x.Model)
                .Include(x => x.PartType)
                .AsQueryable();
            if (id.HasValue && id.Value != Guid.Empty)
                q = q.Where(x => x.Id == id.Value);

            if (modelId.HasValue && modelId.Value != Guid.Empty)
                q = q.Where(x => x.ModelId == modelId.Value);

            if (partTypeId.HasValue && partTypeId.Value != Guid.Empty)
                q = q.Where(x => x.PartTypeId == partTypeId.Value);

            if (status.HasValue)
                q = q.Where(x => x.Status == status);

            var total = await q.LongCountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, total);
        }
    }
}
