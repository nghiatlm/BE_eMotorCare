
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

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
    }
}