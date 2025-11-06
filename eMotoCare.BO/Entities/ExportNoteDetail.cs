

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("export_note_detail")]
    public class ExportNoteDetail
    {
        [Key]
        [Column("export_note_detail_id")]
        public Guid Id { get; set; }
        [Required]
        [Column("export_note_id")]
        public Guid ExportNoteId { get; set; }
        [ForeignKey(nameof(ExportNoteId))]
        public virtual ExportNote? ExportNote { get; set; }
        [Required]
        [Column("part_item_id")]
        public Guid PartItemId { get; set; }
        [ForeignKey(nameof(PartItemId))]
        public virtual PartItem? PartItem { get; set; }
        [Column("note", TypeName = "nvarchar(400)")]
        public string? Note { get; set; }
    }
}
