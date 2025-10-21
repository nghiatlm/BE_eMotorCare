using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.VehicleStageRepository
{
    public interface IVehicleStageRepository : IGenericRepository<VehicleStage>
    {
        Task<List<VehicleStage>> GetByVehicleIdAsync(Guid vehicleId);
    }
}
