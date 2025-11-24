
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("import_note")]
    public class ImportNote : BaseEntity
    {
        [Key]
        [Column("import_note_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("code", TypeName = "nvarchar(100)")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [Column("import_date")]
        public DateTime ImportDate { get; set; }

        [Required]
        [Column("type", TypeName = "varchar(200)")]
        [EnumDataType(typeof(ImportType))]
        public ImportType Type { get; set; }

        [Column("total_amout")]
        public decimal? TotalAmout { get; set; }

        [Required]
        [Column("import_by_id")]
        public Guid? ImportById { get; set; }

        [ForeignKey(nameof(ImportById))]
        public virtual Staff? ImportBy { get; set; }

        [Required]
        [Column("service_center_id")]
        public Guid ServiceCenterId { get; set; }

        [ForeignKey(nameof(ServiceCenterId))]
        public virtual ServiceCenter? ServiceCenter { get; set; }

        // [Column("import_note_status", TypeName = "varchar(200)")]
        // [EnumDataType(typeof(ImportNoteStatus))]
        // public ImportNoteStatus ImportNoteStatus { get; set; }
        public virtual ICollection<ImportNoteDetail>? ImportNoteDetails { get; set; }
    }
}