
using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ExportNoteDetails
{
    public class ExportNoteDetailRepository : GenericRepository<ExportNoteDetail>, IExportNoteDetailRepository
    {
        public ExportNoteDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}