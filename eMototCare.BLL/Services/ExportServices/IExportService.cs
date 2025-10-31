

using eMotoCare.BO.DTO.Requests;
using eMotoCare.BO.DTO.Responses;
using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using eMotoCare.BO.Pages;

namespace eMototCare.BLL.Services.ExportServices
{
    public interface IExportService
    {
        Task<Guid> CreateAsync(ExportNoteRequest req);
        Task DeleteAsync(Guid id);
        Task<ExportNoteResponse?> GetByIdAsync(Guid id);
        Task<PageResult<ExportNoteResponse>> GetPagedAsync(string? code, DateTime? fromDate, DateTime? toDate, ExportType? exportType, string? exportTo, int? totalQuantity, decimal? totalValue, Guid? exportById, Guid? serviceCenterId, ExportNoteStatus? exportNoteStatus, int page, int pageSize);
        Task UpdateAsync(Guid id, ExportNoteUpdateRequest req);
    }
}
