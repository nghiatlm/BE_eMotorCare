using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

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
        [Column("actual_maintenance_mileage")]
        public int ActualMaintenanceMileage { get; set; }

        [Required]
        [Column("actual_maintenance_unit", TypeName = "varchar(200)")]
        [EnumDataType(typeof(MaintenanceUnit))]
        public MaintenanceUnit ActualMaintenanceUnit { get; set; }

        [Required]
        [Column("vehicle_id")]
        public Guid VehicleId { get; set; }

        [Required]
        [Column("date_of_implementation")]
        public DateTime DateOfImplementation { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(VehicleStageStatus))]
        public VehicleStageStatus Status { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle? Vehicle { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
