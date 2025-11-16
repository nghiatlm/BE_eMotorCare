using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ExportNoteRepository
{
    public class ExportNoteRepository : GenericRepository<ExportNote>, IExportNoteRepository
    {
        public ExportNoteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<ExportNote> Items, long Total)> GetPagedAsync(
             string? code,
             DateTime? startDate,
             DateTime? endDate,
             ExportType? exportType,
             string? exportTo,
             int? totalQuantity,
             decimal? totalValue,
             Guid? exportById,
             Guid? serviceCenterId,
             ExportNoteStatus? exportNoteStatus,
             Guid? partItemId,
             int page,
             int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.ExportNotes
                .Include(x => x.ExportBy)
                .Include(x => x.ServiceCenter)
                .Include(x => x.PartItems)
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(code))
                q = q.Where(x => x.Code.Contains(code));

            if (startDate.HasValue && endDate.HasValue)
            {
                var endDateInclusive = endDate.Value.Date.AddDays(1);
                q = q.Where(x => x.ExportDate >= startDate.Value.Date && x.ExportDate < endDateInclusive);
            }
            else if (startDate.HasValue)
            {
                var todayEnd = DateTime.Now.Date.AddDays(1);
                q = q.Where(x => x.ExportDate >= startDate.Value.Date && x.ExportDate < todayEnd);
            }
            else if (endDate.HasValue)
            {
                var endDateInclusive = endDate.Value.Date.AddDays(1);
                q = q.Where(x => x.ExportDate < endDateInclusive);
            }


            if (exportType.HasValue)
                q = q.Where(x => x.Type == exportType.Value);

            if (!string.IsNullOrWhiteSpace(exportTo))
                q = q.Where(x => x.ExportTo != null && x.ExportTo.Contains(exportTo));

            if (totalQuantity.HasValue)
                q = q.Where(x => x.TotalQuantity == totalQuantity.Value);

            if (totalValue.HasValue)
                q = q.Where(x => x.TotalValue == totalValue.Value);

            if (exportById.HasValue)
                q = q.Where(x => x.ExportById == exportById.Value);

            if (serviceCenterId.HasValue)
                q = q.Where(x => x.ServiceCenterId == serviceCenterId.Value);

            if (exportNoteStatus.HasValue)
                q = q.Where(x => x.ExportNoteStatus == exportNoteStatus.Value);

            if (partItemId.HasValue)
                q = q.Where(x => x.PartItems.Any(p => p.Id == partItemId.Value));

            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<ExportNote?> GetByIdAsync(Guid id) =>
        _context.ExportNotes
            .Include(x => x.ExportBy)
            .Include(x => x.ServiceCenter)
            .Include(x => x.PartItems)
            .FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.ExportNotes.AnyAsync(x => x.Code == code);
    }
}
