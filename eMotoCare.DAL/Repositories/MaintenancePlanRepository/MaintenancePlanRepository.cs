using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.MaintenancePlanRepository
{
    public class MaintenancePlanRepository : GenericRepository<MaintenancePlan>, IMaintenancePlanRepository
    {
        public MaintenancePlanRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
