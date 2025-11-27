

using eMotoCare.BO.DTO.Requests;

namespace eMototCare.BLL.Services.ExportNoteDetailServices
{
    public interface IExportNoteDetailService
    {
        Task UpdateAsync(Guid id, ExportNoteDetailUpdateRequest req);
    }
}
