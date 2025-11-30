using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ExportNoteRepository
{
    public class ExportNoteRepository : GenericRepository<ExportNote>, IExportNoteRepository
    {
        public ExportNoteRepository(ApplicationDbContext context)
            : base(context) { }

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
            bool outOfStock = false,
            int page = 1,
            int pageSize = 10
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context
                .ExportNotes.Include(x => x.ExportBy)
                .Include(x => x.ServiceCenter)
                .Include(x => x.ExportNoteDetails)
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(code))
                q = q.Where(x => x.Code.Contains(code));

            if (startDate.HasValue && endDate.HasValue)
            {
                var endDateInclusive = endDate.Value.Date.AddDays(1);
                q = q.Where(x =>
                    x.ExportDate >= startDate.Value.Date && x.ExportDate < endDateInclusive
                );
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
            if (outOfStock)
            {
                q = q.Where(x =>
                    x.ExportNoteDetails.Any(d => d.Status == ExportNoteDetailStatus.STOCK_NOT_FOUND)
                );
            }

            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<ExportNote?> GetByIdAsync(Guid id)
        {
            return _context
                .ExportNotes.Include(x => x.ExportBy)
                .Include(x => x.ServiceCenter)
                .Include(x => x.ExportNoteDetails)
                .ThenInclude(xx => xx.ProposedReplacePart)
                .Include(x => x.ExportNoteDetails)
                .ThenInclude(xx => xx.PartItem)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.ExportNotes.AnyAsync(x => x.Code == code);

        public Task<ExportNote> FindByNote(string note) =>
            _context
                .ExportNotes.Include(x => x.ExportNoteDetails)
                .FirstOrDefaultAsync(x => x.Note.Contains(note));

        public Task<ExportNote?> GetByOutOfStock(Guid id)
        {
            return _context
                .ExportNotes.Include(x => x.ExportBy)
                .Include(x => x.ServiceCenter)
                .Include(x =>
                    x.ExportNoteDetails.Where(d => d.Status == ExportNoteDetailStatus.STOCK_NOT_FOUND)
                )
                .ThenInclude(xx => xx.ProposedReplacePart)
                .Include(x =>
                    x.ExportNoteDetails.Where(d => d.Status == ExportNoteDetailStatus.STOCK_NOT_FOUND)
                )
                .ThenInclude(xx => xx.PartItem)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
