using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ImportNoteRepository
{
    public class ImportNoteRepository : GenericRepository<ImportNote>, IImportNoteRepository
    {
        public ImportNoteRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
