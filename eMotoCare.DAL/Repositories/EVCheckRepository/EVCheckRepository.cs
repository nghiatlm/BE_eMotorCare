using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.EVCheckRepository
{
    public class EVCheckRepository : GenericRepository<EVCheck>, IEVCheckRepository
    {
        public EVCheckRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
