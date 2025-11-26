using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("export_note")]
    public class ExportNote : BaseEntity
    {
        [Key]
        [Column("export_note_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("code", TypeName = "nvarchar(100)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column("export_date")]
        public DateTime ExportDate { get; set; }

        [Required]
        [Column("type", TypeName = "varchar(200)")]
        [EnumDataType(typeof(ExportType))]
        public ExportType Type { get; set; }

        [Column("export_to")]
        public string? ExportTo { get; set; }

        [Required]
        [Column("total_quantity")]
        public int TotalQuantity { get; set; }

        [Required]
        [Column("total_value")]
        public decimal TotalValue { get; set; }

        [Column("note", TypeName = "nvarchar(400)")]
        public string? Note { get; set; }

        [Column("export_by_id")]
        public Guid? ExportById { get; set; }

        [ForeignKey(nameof(ExportById))]
        public virtual Staff? ExportBy { get; set; }

        [Required]
        [Column("service_center_id")]
        public Guid ServiceCenterId { get; set; }

        [Column("export_note_status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(ExportNoteStatus))]
        public ExportNoteStatus ExportNoteStatus { get; set; }

        [ForeignKey(nameof(ServiceCenterId))]
        public virtual ServiceCenter? ServiceCenter { get; set; }
        public virtual ICollection<ExportNoteDetail>? ExportNoteDetails { get; set; }
        [Column("total_exports")]
        public int TotalExports { get; set; }

    }
}