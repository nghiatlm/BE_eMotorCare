
using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("model_part_type")]
    public class ModelPartType
    {
        [Key]
        [Column("model_part_type_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("model_id")]
        public Guid ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public virtual Model? Model { get; set; }

        [Required]
        [Column("part_type_id")]
        public Guid PartTypeId { get; set; }

        [ForeignKey(nameof(PartTypeId))]
        public virtual PartType? PartType { get; set; }

        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
    }
}