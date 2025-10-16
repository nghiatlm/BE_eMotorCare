using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("part_type")]
    public class PartType : BaseEntity
    {
        [Key]
        [Column("part_type_id")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("description")]
        [StringLength(2000)]
        public string? Description { get; set; }
        public virtual ICollection<Part>? Parts { get; set; }
        public virtual ICollection<PriceService>? PriceServices { get; set; }
        public virtual ICollection<ModelPartType>? ModelPartTypes { get; set; }
    }
}
