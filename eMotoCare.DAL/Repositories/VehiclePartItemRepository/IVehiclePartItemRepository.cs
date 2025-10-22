using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.VehiclePartItemRepository
{
    public interface IVehiclePartItemRepository : IGenericRepository<VehiclePartItem>
    {
        Task<(IReadOnlyList<VehiclePartItem> Items, long Total)> GetPagedAsync(
            string? search,
            Guid? vehicleId,
            Guid? partItemId,
            Guid? replaceForId,
            DateTime? fromInstallDate,
            DateTime? toInstallDate,
            int page,
            int pageSize
        );

        Task<VehiclePartItem?> GetByIdAsync(Guid id);
    }
}
