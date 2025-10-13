
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class ExportNote : BaseEntity
    {
        public Guid ExportNoteId { get; set; }
        public Guid BranchId { get; set; }
        public Branch? Branch { get; set; }
        public DateTime ExportDate { get; set; }
        public int TotalAmount { get; set; }
        public int NumberOfRequest { get; set; }
        public Status Status { get; set; }
        public string? Note { get; set; }

        public virtual ICollection<BranchInventoryExport>? BranchInventoryExports { get; set; }
        public virtual ICollection<PartItem>? PartItems { get; set; }


    }
}