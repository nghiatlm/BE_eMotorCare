
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class PartItem : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid PartId { get; set; }
        public string SerialNumber { get; set; }
        public Status Status { get; set; }
        public decimal Price { get; set; }
        public DateTime WarrantyExpire { get; set; }
        public Guid? ExportNoteId { get; set; }
        public Guid? ImportNoteId { get; set; }
        public Guid BranchInventoryId { get; set; }

        public virtual Part? Part { get; set; }
        public virtual ExportNote? ExportNote { get; set; }
        public virtual ImportNote? ImportNote { get; set; }
        public virtual BranchInventory? BranchInventory { get; set; }
    }
}