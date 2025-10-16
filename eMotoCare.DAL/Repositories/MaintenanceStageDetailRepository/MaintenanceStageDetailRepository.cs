using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.MaintenanceStageDetailRepository
{
    public class MaintenanceStageDetailRepository : GenericRepository<MaintenanceStageDetail>, IMaintenanceStageDetailRepository
    {
        public MaintenanceStageDetailRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
