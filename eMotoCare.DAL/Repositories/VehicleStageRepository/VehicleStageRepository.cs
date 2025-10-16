using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;

namespace eMotoCare.DAL.Repositories.VehicleStageRepository
{
    public class VehicleStageRepository : GenericRepository<VehicleStage>, IVehicleStageRepository
    {
        public VehicleStageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
