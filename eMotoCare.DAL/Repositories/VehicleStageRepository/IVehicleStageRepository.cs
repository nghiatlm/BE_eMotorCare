using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.VehicleStageRepository
{
    public interface IVehicleStageRepository : IGenericRepository<VehicleStage>
    {
        Task<List<VehicleStage>> GetByVehicleIdAsync(Guid vehicleId);
        Task<VehicleStage?> GetByIdAsync(Guid id);

        Task<(IReadOnlyList<VehicleStage> Items, long Total)> GetPagedAsync(
            Guid? vehicleId,
            Guid? maintenanceStageId,
            VehicleStageStatus? status,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize
        );
    }
}
