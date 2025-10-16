using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.MaintenanceStageRepository
{
    public class MaintenanceStageRepository : GenericRepository<MaintenanceStage>, IMaintenanceStageRepository
    {
        public MaintenanceStageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
