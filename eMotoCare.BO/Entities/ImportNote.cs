
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("import_note")]
    public class ImportNote
    {
        [Key]
        [Column("import_note_id")]
        public Guid Id { get; set; }
        [Required]
        [Column("import_by_id")]
        public Guid ImportById { get; set; }

        [ForeignKey(nameof(ImportById))]
        public virtual Staff? ImportBy { get; set; }

        [Required]
        [Column("service_center_id")]
        public Guid ServiceCenterId { get; set; }

        [ForeignKey(nameof(ServiceCenterId))]
        public virtual ServiceCenter? ServiceCenter { get; set; }

        public virtual ICollection<PartItem>? PartItems { get; set; }
    }
}