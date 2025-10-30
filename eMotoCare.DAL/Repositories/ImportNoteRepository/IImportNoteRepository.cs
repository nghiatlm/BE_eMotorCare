using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ImportNoteRepository
{
    public interface IImportNoteRepository : IGenericRepository<ImportNote>
    {
        Task<bool> ExistsCodeAsync(string code);
        Task<ImportNote?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<ImportNote> Items, long Total)> GetPagedAsync(string? code, DateTime? startDate, DateTime? endDate, string? importFrom, string? supplier, ImportType? importType, decimal? totalAmount, Guid? importById, Guid? serviceCenterId, ImportNoteStatus? importNoteStatus, int page, int pageSize);
    }
}
