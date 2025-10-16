using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using eMotoCare.DAL.Repositories.BatteryCheckRepository;

namespace eMotoCare.DAL.Repositories.BatteryCheckRepository
{
    public class BatteryCheckRepository : GenericRepository<BatteryCheck>, IBatteryCheckRepository
    {
        public BatteryCheckRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
