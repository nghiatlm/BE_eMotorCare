
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Guid ExportNoteDetailId { get; set; }

        [ForeignKey(nameof(ExportNoteDetailId))]
        public virtual ExportNote? ExportNote { get; set; }

        
        [Column("part_item_id")]
        public Guid? PartItemId { get; set; }

        [ForeignKey(nameof(PartItemId))]
        public virtual PartItem? PartItem { get; set; }

        [Column("proposed_replace_part_id")]
        public Guid? ProposedReplacePartId { get; set; }

        [ForeignKey(nameof(ProposedReplacePartId))]
        public virtual Part? ProposedReplacePart { get; set; }

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

        [Column("export_index")]
        public int? ExportIndex { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(ExportNoteDetailStatus))]
        public ExportNoteDetailStatus Status { get; set; }
    }
}