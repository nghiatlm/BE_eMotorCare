using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Common;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("vehicle_stage")]
    public class VehicleStage : BaseEntity
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

        [Column("expected_implementation_date")]
        public DateTime? ExpectedImplementationDate { get; set; }

        [Column("expected_start_date")]
        public DateTime? ExpectedStartDate { get; set; }

        [Column("expected_end_date")]
        public DateTime? ExpectedEndDate { get; set; }

        [Column("actual_implementation_date")]
        public DateTime? ActualImplementationDate { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(VehicleStageStatus))]
        public VehicleStageStatus Status { get; set; }

        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle? Vehicle { get; set; }

        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
