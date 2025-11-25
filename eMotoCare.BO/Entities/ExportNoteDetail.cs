
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("export_note_detail")]
    public class ExportNoteDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("export_note_detail_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("export_note_id")]
        public Guid ImportNoteId { get; set; }

        [ForeignKey(nameof(ImportNoteId))]
        public virtual ExportNote? ExportNote { get; set; }

        [Required]
        [Column("part_item_id")]
        public Guid PartItemId { get; set; }

        [ForeignKey(nameof(PartItemId))]
        public virtual PartItem? PartItem { get; set; }

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Required]
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("note", TypeName = "nvarchar(300)")]
        public string? Note { get; set; }
    }
}