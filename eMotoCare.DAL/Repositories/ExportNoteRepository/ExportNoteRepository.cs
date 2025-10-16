using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ExportNoteRepository
{
    public class ExportNoteRepository : GenericRepository<ExportNote>, IExportNoteRepository
    {
        public ExportNoteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
