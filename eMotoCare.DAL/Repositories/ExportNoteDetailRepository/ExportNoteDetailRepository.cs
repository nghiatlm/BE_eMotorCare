

using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ExportNoteDetailRepository
{
    public class ExportNoteDetailRepository : GenericRepository<ExportNoteDetail>, IExportNoteDetailRepository
    {
        public ExportNoteDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<ExportNoteDetail> Items, long Total)> GetPagedAsync(
            Guid? exportNoteId,
            Guid? partItemId,
            int page,
            int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.ExportNoteDetails
                .Include(x => x.PartItem)
                    .ThenInclude(x => x.Part)
                .Include(x => x.ExportNote)
                .AsNoTracking()
                .AsQueryable();
            if (exportNoteId.HasValue)
                q = q.Where(x => x.ExportNoteId == exportNoteId.Value);

            if (partItemId.HasValue)
                q = q.Where(x => x.PartItemId == partItemId.Value);




            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.ExportNote.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<ExportNoteDetail?> GetByIdAsync(Guid id) =>
        _context.ExportNoteDetails
            .Include(x => x.PartItem)
                .ThenInclude(x => x.Part)
            .Include(x => x.ExportNote)
            .FirstOrDefaultAsync(x => x.Id == id);

    }
}
