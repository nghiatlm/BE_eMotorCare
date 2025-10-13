
using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace eMotoCare.Domain.Entities
{
    [Index(nameof(SerialNumber), IsUnique = true)]
    public class PartItem : BaseEntity
    {
        public Guid PartItemId { get; set; }
        public Guid PartId { get; set; }
        public string SerialNumber { get; set; }
        public Status Status { get; set; }
        public decimal Price { get; set; }
        public DateTime WarrantyExpire { get; set; }
        public Guid? ExportNoteId { get; set; }
        public Guid? ImportNoteId { get; set; }
        public Guid BranchInventoryId { get; set; }

        public Part? Part { get; set; }
        public ExportNote? ExportNote { get; set; }
        public ImportNote? ImportNote { get; set; }
        public BranchInventory? BranchInventory { get; set; }

        public virtual ICollection<VehiclePartItem>? VehiclePartItems { get; set; }
        public virtual ICollection<EVCheckDetail>? EVCheckDetails { get; set; }
    }
}