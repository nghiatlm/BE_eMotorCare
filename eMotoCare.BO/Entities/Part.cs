using eMotoCare.BO.Common;
using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("part")]
    public class Part : BaseEntity
    {
        [Key]
        [Column("part_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("part_type_id")]
        public Guid PartTypeId { get; set; }

        [ForeignKey(nameof(PartTypeId))]
        public virtual PartType? PartType { get; set; }

        [Required]
        [Column("code")]
        public string Code { get; set; } = default!;

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = default!;

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("image")]
        [StringLength(500)]
        public string? Image { get; set; }
        [Column("description", TypeName = "nvarchar(300)")]
        public string? Description { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        [InverseProperty(nameof(EVCheckDetail.ProposedReplacePart))]
        public virtual EVCheckDetail? ProposedReplacePart { get; set; }


        public virtual ICollection<PartItem>? PartItems { get; set; }
        public virtual ICollection<ProgramDetail>? ProgramDetails { get; set; }
        public virtual ICollection<MaintenanceStageDetail>? MaintenanceStageDetails { get; set; }
        public virtual ICollection<ModelPart>? ModelParts { get; set; }
    }
}
