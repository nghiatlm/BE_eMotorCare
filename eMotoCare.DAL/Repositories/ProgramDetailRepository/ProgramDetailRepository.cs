

using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ProgramDetailRepository
{
    public class ProgramDetailRepository : GenericRepository<ProgramDetail>, IProgramDetailRepository
    {
        public ProgramDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}