using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("export_not")]
    public class ExportNote
    {
        [Key]
        [Column("export_note_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("export_by_id")]
        public Guid ExportById { get; set; }

        [ForeignKey(nameof(ExportById))]
        public virtual Staff? ExportBy { get; set; }

        [Required]
        [Column("service_center_id")]
        public Guid ServiceCenterId { get; set; }

        [ForeignKey(nameof(ServiceCenterId))]
        public virtual ServiceCenter? ServiceCenter { get; set; }

        public virtual ICollection<PartItem>? PartItems { get; set; }
    }
}