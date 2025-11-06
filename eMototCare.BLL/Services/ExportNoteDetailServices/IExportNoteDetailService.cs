
using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ExportNoteDetailServices
{
    public interface IExportNoteDetailService
    {
        Task<Guid> CreateAsync(ExportNoteDetailRequest req);
        Task DeleteAsync(Guid id);
        Task<ExportNoteDetailResponse?> GetByIdAsync(Guid id);
        Task<PageResult<ExportNoteDetailResponse>> GetPagedAsync(Guid? exportNoteId, Guid? partItemId, int page, int pageSize);
        Task UpdateAsync(Guid id, ExportNoteDetailRequest req);
    }
}
