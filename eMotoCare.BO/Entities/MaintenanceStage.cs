using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enum;

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

        [Required]
        [StringLength(255)]
        [Column("name")]
        public string Name { get; set; } = default!;

        [Column("description")]
        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        [Column("mileage")]
        public Mileage Mileage { get; set; }

        [Required]
        [Column("duration_month")]
        public DurationMonth DurationMonth { get; set; }

        [Column("estimated_time")]
        public int? EstimatedTime { get; set; }

        [Required]
        [Column("status")]
        public Status Status { get; set; }

        public virtual ICollection<VehicleStage>? VehicleStages { get; set; }
        public virtual ICollection<MaintenanceStageDetail>? MaintenanceStageDetails { get; set; }
    }
}
