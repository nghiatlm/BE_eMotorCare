

using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class ExportNoteDetailUpdateRequest
    {
        public Guid? PartItemId { get; set; }
        public string? Note { get; set; }
        public int? ExportIndex { get; set; }
        public ExportNoteDetailStatus? Status { get; set; }
    }
}
