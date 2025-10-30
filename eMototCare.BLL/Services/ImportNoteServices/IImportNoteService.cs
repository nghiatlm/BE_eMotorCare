

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ImportNoteServices
{
    public interface IImportNoteService
    {
        Task<Guid> CreateAsync(ImportNoteRequest req);
        Task DeleteAsync(Guid id);
        Task<ImportNoteResponse?> GetByIdAsync(Guid id);
        Task<PageResult<ImportNoteResponse>> GetPagedAsync(string? code, DateTime? fromDate, DateTime? toDate, string? importFrom, string? supplier, ImportType? importType, decimal? totalAmount, Guid? importById, Guid? serviceCenterId, ImportNoteStatus? importNoteStatus, int page, int pageSize);
        Task UpdateAsync(Guid id, ImportNoteUpdateRequest req);
    }
}
