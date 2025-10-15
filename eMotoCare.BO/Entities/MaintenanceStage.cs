
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;

namespace eMotoCare.BO.Entities
{
    [Table("maintenance_stage")]
    public class MaintenanceStage : BaseEntity
    {
        [Key]
        [Column("maintenance_stage_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("maintenance_plan_id")]
        public Guid MaintenancePlanId { get; set; }

        [ForeignKey(nameof(MaintenancePlanId))]
        public virtual MaintenancePlan? MaintenancePlan { get; set; }
        public virtual ICollection<VehicleStage>? VehicleStages { get; set; }
        public virtual ICollection<MaintenanceStageDetail>? MaintenanceStageDetails { get; set; }
    }
}