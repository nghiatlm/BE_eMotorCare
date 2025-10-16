using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.PriceServiceRepository
{
    public class PriceServiceRepository : GenericRepository<PriceService>, IPriceServiceRepository
    {
        public PriceServiceRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
