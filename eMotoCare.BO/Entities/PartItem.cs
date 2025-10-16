using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.Entities
{
    [Table("part_item")]
    public class PartItem
    {
        [Key]
        [Column("part_item_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("part_id")]
        public Guid PartId { get; set; }

        [ForeignKey(nameof(PartId))]
        public virtual Part? Part { get; set; }

        [Column("export_note_id")]
        public Guid? ExportNoteId { get; set; }

        [ForeignKey(nameof(ExportNoteId))]
        public virtual ExportNote? ExportNote { get; set; }

        [Column("import_note_id")]
        public Guid? ImportNoteId { get; set; }

        [ForeignKey(nameof(ImportNoteId))]
        public virtual ImportNote? ImportNote { get; set; }

        [InverseProperty(nameof(EVCheckDetail.ReplacePart))]
        public virtual EVCheckDetail? ReplcePart { get; set; }

        [InverseProperty(nameof(VehiclePartItem.ReplaceFor))]
        public virtual VehiclePartItem? ReplaceFor { get; set; }

        [InverseProperty(nameof(ServiceCenterInventory.PartItem))]
        [Column("export_id")]
        public Guid? ExportId { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [StringLength(100)]
        [Column("serial_number")]
        public string SerialNumber { get; set; } = default!;

        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column("status")]
        public Status Status { get; set; }

        [Column("warranty_period")]
        public int? WarrantyPeriod { get; set; }

        public virtual ServiceCenterInventory? ServiceCenterInventory { get; set; }
        public virtual ICollection<RMADetail>? RMADetails { get; set; }
        public virtual ICollection<VehiclePartItem>? VehiclePartItems { get; set; }
        public virtual ICollection<BatteryCheck>? BatteryChecks { get; set; }
        public virtual ICollection<EVCheckDetail>? EVCheckDetails { get; set; }
    }
}
