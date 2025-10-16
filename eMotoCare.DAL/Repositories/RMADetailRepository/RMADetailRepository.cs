using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.RMADetailRepository
{
    public class RMADetailRepository : GenericRepository<RMADetail>, IRMADetailRepository
    {
        public RMADetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
