

using eMotoCare.BO.Entities;
using eMotoCare.DAL.Base;

namespace eMotoCare.DAL.Repositories.ExportNoteDetailRepository
{
    public interface IExportNoteDetailRepository : IGenericRepository<ExportNoteDetail>
    {
        Task<ExportNoteDetail?> GetByIdAsync(Guid id);
        Task<(IReadOnlyList<ExportNoteDetail> Items, long Total)> GetPagedAsync(Guid? exportNoteId, Guid? partItemId, int page, int pageSize);
    }
}
