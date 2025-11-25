
using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ImportNoteDetails
{
    public class ImportNoteDetailRepository : GenericRepository<ImportNoteDetail>, IImportNoteDetailRepository
    {
        public ImportNoteDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}