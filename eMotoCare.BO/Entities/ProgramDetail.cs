
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.Entities
{
    [Table("program_detail")]
    public class ProgramDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("program_detail_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("program_id")]
        public Guid ProgramId { get; set; }

        [ForeignKey(nameof(ProgramId))]
        public virtual Program Program { get; set; } = null!;

        [Column("part_id")]
        public Guid? PartId { get; set; }

        [ForeignKey(nameof(PartId))]
        public virtual Part? Part { get; set; }

        [Required]
        [Column("action_type", TypeName = "varchar(20)")]
        [EnumDataType(typeof(ActionType))]
        public ActionType ActionType { get; set; }

        [Column("description", TypeName = "longtext")]
        public string? Description { get; set; }

        [Column("manufacture_year")]
        public int? ManufactureYear { get; set; }

        [Column("model_id")]
        public Guid? ModelId { get; set; }

        [ForeignKey(nameof(ModelId))]
        public virtual Model? Model { get; set; }
    }
}