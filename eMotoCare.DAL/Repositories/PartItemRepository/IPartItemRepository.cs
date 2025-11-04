using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.PartItemRepository
{
    public interface IPartItemRepository : IGenericRepository<PartItem>
    {
        Task<bool> ExistsSerialNumberAsync(string serialNumber);
        Task<List<PartItem>> GetByExportNoteIdAsync(Guid exportNoteId);
        Task<PartItem?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<PartItem> Items, long Total)> GetPagedAsync(Guid? partId, Guid? exportNoteId, Guid? importNoteId, string? serialNumber, PartItemStatus? status, Guid? serviceCenterInventoryId, int page, int pageSize);
    }
}
