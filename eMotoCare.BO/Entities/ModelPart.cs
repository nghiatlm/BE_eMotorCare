
using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("model_part")]
    public class ModelPart
    {
        [Key]
        [Column("model_part_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("model_id")]
        public Guid ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public virtual Model? Model { get; set; }

        [Required]
        [Column("part_id")]
        public Guid PartId { get; set; }

        [ForeignKey(nameof(PartId))]
        public virtual Part? Part { get; set; }


    }
}