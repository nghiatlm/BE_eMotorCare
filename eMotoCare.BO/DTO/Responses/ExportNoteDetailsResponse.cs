using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class ExportNoteDetailsResponse
    {
        public Guid Id { get; set; }
        public Guid ExportNoteDetailId { get; set; }
        public Guid? PartItemId { get; set; }
        public virtual PartItem? PartItem { get; set; }
        public Guid? ProposedReplacePartId { get; set; }
        public virtual Part? ProposedReplacePart { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Note { get; set; }
        public int? ExportIndex { get; set; }
        public ExportNoteDetailStatus Status { get; set; }
    }
}