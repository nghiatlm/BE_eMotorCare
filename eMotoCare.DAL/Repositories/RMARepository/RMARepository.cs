using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.RMARepository
{
    public class RMARepository : GenericRepository<RMA>, IRMARepository
    {
        public RMARepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
