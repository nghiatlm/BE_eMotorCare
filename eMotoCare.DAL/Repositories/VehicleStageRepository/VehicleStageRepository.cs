using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;
using eMotoCare.DAL.context;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.DAL.Repositories.VehicleStageRepository
{
    public class VehicleStageRepository : GenericRepository<VehicleStage>, IVehicleStageRepository
    {
        public VehicleStageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<List<VehicleStage>> GetByVehicleIdAsync(Guid vehicleId)
        {
            return _context.VehicleStages
                           .Where(vs => vs.VehicleId == vehicleId)
                           .ToListAsync();
        }
    }
}
