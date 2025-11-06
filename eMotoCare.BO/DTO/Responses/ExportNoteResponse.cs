

using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class ExportNoteResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime ExportDate { get; set; }
        public ExportType Type { get; set; }
        public string? ExportTo { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public string? Note { get; set; }
        public StaffResponse? ExportBy { get; set; }
        public ServiceCenterResponse? ServiceCenter { get; set; }
        public ICollection<ExportNoteDetailResponse>? ExportNoteDetails { get; set; }
        public ExportNoteStatus ExportNoteStatus { get; set; }

    }
}
