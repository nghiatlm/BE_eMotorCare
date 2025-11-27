
using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.ExportNoteDetails
{
    public class ExportNoteDetailRepository : GenericRepository<ExportNoteDetail>, IExportNoteDetailRepository
    {
        public ExportNoteDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ExportNoteDetail?> GetByIdAsync(Guid id)
        {
            var exportNoteDetail = await _context
                .ExportNoteDetails
                .Include(x => x.ExportNote)
                    .ThenInclude(x => x.ExportNoteDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
            return exportNoteDetail;
        }
    }
}