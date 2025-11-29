

using eMotoCare.BO.DTO.Requests;

namespace eMototCare.BLL.Services.ExportNoteDetailServices
{
    public interface IExportNoteDetailService
    {
        Task<string> GetExportStatus(string appointmentCode, Guid proposedPartId);
        Task UpdateAsync(Guid id, ExportNoteDetailUpdateRequest req);
    }
}
