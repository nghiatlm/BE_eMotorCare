using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ServiceCenterInventoryRepository
{
    public interface IServiceCenterInventoryRepository : IGenericRepository<ServiceCenterInventory>
    {
        Task<ServiceCenterInventory?> GetByIdAsync(Guid id);
        Task<ServiceCenterInventory?> GetByServiceCenterId(Guid id);
        Task<(IReadOnlyList<ServiceCenterInventory> Items, long Total)> GetPagedAsync(Guid? serviceCenterId, string? serviceCenterInventoryName, Status? status, string? partCode, int page, int pageSize);
    }
}
