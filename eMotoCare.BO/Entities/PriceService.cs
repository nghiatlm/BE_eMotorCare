using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.Entities
{
    [Table("price_service")]
    public class PriceService : BaseEntity
    {
        [Key]
        [Column("price_service_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("part_type_id")]
        public Guid PartTypeId { get; set; }

        [ForeignKey(nameof(PartTypeId))]
        public virtual PartType? PartType { get; set; }

        [Required]
        [StringLength(50)]
        [Column("code")]
        public string Code { get; set; } = default!;

        [Required]
        [Column("remedies", TypeName = "varchar(200)")]
        public Remedies Remedies { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = default!;

        [Required]
        [Column("labor_cost", TypeName = "decimal(18,2)")]
        public decimal LaborCost { get; set; }

        [Required]
        [Column("effective_date")]
        public DateTime EffectiveDate { get; set; }

        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column("description")]
        [StringLength(2000)]
        public string? Description { get; set; }

        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
    }
}
