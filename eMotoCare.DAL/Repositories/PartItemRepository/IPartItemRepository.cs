using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.PartItemRepository
{
    public interface IPartItemRepository : IGenericRepository<PartItem>
    {
        Task<bool> ExistsSerialNumberAsync(string serialNumber);
        Task<PartItem?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<PartItem> Items, long Total)> GetPagedAsync(Guid? partId, string? serialNumber, PartItemStatus? status, Guid? serviceCenterInventoryId, int page, int pageSize);
        Task<List<PartItem>> GetByServiceCenterIdAsync(Guid serviceCenterId);
    }
}
