using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ImportNoteRepository
{
    public class ImportNoteRepository : GenericRepository<ImportNote>, IImportNoteRepository
    {
        public ImportNoteRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IReadOnlyList<ImportNote> Items, long Total)> GetPagedAsync(
             string? code,
             DateTime? startDate,
             DateTime? endDate,
             string? importFrom,
             string? supplier,
             ImportType? importType,
             decimal? totalAmount,
             Guid? importById,
             Guid? serviceCenterId,
             ImportNoteStatus? importNoteStatus,
             int page,
             int pageSize
        )
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var q = _context.ImportNotes
                .Include(x => x.ImportBy)
                .Include(x => x.ServiceCenter)
                .AsNoTracking()
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(code))
                q = q.Where(x => x.Code.Contains(code));

            if (startDate.HasValue && endDate.HasValue)
            {
                var endDateInclusive = endDate.Value.Date.AddDays(1);
                q = q.Where(x => x.ImportDate >= startDate.Value.Date && x.ImportDate < endDateInclusive);
            }
            else if (startDate.HasValue)
            {
                var todayEnd = DateTime.Now.Date.AddDays(1);
                q = q.Where(x => x.ImportDate >= startDate.Value.Date && x.ImportDate < todayEnd);
            }
            else if (endDate.HasValue)
            {
                var endDateInclusive = endDate.Value.Date.AddDays(1);
                q = q.Where(x => x.ImportDate < endDateInclusive);
            }

            if (!string.IsNullOrWhiteSpace(importFrom))
                q = q.Where(x => x.ImportFrom.Contains(importFrom));

            if (!string.IsNullOrWhiteSpace(supplier))
                q = q.Where(x => x.Supplier.Contains(supplier));

            if (importType.HasValue)
                q = q.Where(x => x.Type == importType.Value);

            if (totalAmount.HasValue)
                q = q.Where(x => x.TotalAmout == totalAmount.Value);

            if (importById.HasValue)
                q = q.Where(x => x.ImportById == importById.Value);

            if (serviceCenterId.HasValue)
                q = q.Where(x => x.ServiceCenterId == serviceCenterId.Value);

            if (importNoteStatus.HasValue)
                q = q.Where(x => x.ImportNoteStatus == importNoteStatus.Value);



            var total = await q.LongCountAsync();

            var items = await q.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public Task<ImportNote?> GetByIdAsync(Guid id) =>
        _context.ImportNotes.FirstOrDefaultAsync(x => x.Id == id);

        public Task<bool> ExistsCodeAsync(string code) =>
            _context.ImportNotes.AnyAsync(x => x.Code == code);
    }
}
