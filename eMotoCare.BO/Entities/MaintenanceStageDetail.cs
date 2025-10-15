
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("maintenance_stage_detail")]

    public class MaintenanceStageDetail : BaseEntity
    {
        [Key]
        [Column("maintenance_stage_detail_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("maintenance_stage_id")]
        public Guid MaintenanceStageId { get; set; }

        [ForeignKey(nameof(MaintenanceStageId))]
        public virtual MaintenanceStage? MaintenanceStage { get; set; }

        [Required]
        [Column("part_id")]
        public Guid PartId { get; set; }

        [ForeignKey(nameof(PartId))]
        public virtual Part? Part { get; set; }

        [InverseProperty(nameof(EVCheckDetail.MaintenanceStageDetail))]
        public virtual EVCheckDetail? EVCheckDetail { get; set; }
    }
}