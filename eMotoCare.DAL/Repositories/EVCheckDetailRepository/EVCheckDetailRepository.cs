using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.EVCheckDetailRepository
{
    public class EVCheckDetailRepository : GenericRepository<EVCheckDetail>, IEVCheckDetailRepository
    {
        public EVCheckDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
