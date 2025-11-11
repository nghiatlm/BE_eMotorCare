using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.VehicleRepository
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<(IReadOnlyList<Vehicle> Items, long Total)> GetPagedAsync(
            string? search,
            StatusEnum? status,
            Guid? modelId,
            Guid? customerId,
            DateTime? fromPurchaseDate,
            DateTime? toPurchaseDate,
            int page,
            int pageSize
        );

        Task<Vehicle?> GetByIdAsync(Guid id);

        Task<Model?> GetModelByIdAsync(Guid modelId);
        Task<Vehicle?> GetByVinNumber(string vinNumber);
    }
}
