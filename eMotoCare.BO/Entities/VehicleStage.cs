
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("vehicle_stage")]
    public class VehicleStage
    {
        [Key]
        [Column("vehicle_stage_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("maintenance_stage_id")]
        public Guid MaintenanceStageId { get; set; }

        [ForeignKey(nameof(MaintenanceStageId))]
        public virtual MaintenanceStage? MaintenanceStage { get; set; }

        [Required]
        [Column("vehicle_id")]
        public Guid VehicleId { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle? Vehicle { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}