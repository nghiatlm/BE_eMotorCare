
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("program_detail")]
    public class ProgramDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("program_detail_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("program_id")]
        public Guid ProgramId { get; set; }

        [ForeignKey(nameof(ProgramId))]
        public virtual Program? Program { get; set; }

        [Column("part_id")]
        public Guid? RecallPartId { get; set; }

        [ForeignKey(nameof(RecallPartId))]
        public virtual Part? RecallPart { get; set; }
        [Column("service_type", TypeName = "varchar(100)")]
        public string? ServiceType { get; set; }

        [Column("discount_percent")]
        public int? DiscountPercent { get; set; }

        [Column("bonus_amount")]
        public int? BonusAmount { get; set; }

        [Column("recall_action", TypeName = "longtext")]
        public string? RecallAction { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}