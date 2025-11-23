

using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.ProgramModelRepository
{
    public class ProgramModelRepository : GenericRepository<ProgramModel>, IProgramModelRepository
    {
        public ProgramModelRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}