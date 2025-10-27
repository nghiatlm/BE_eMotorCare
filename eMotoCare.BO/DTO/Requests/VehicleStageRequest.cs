using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class VehicleStageRequest
    {
        [Required]
        public Guid MaintenanceStageId { get; set; }

        [Required]
        public int ActualMaintenanceMileage { get; set; }

        [Required]
        public MaintenanceUnit ActualMaintenanceUnit { get; set; }

        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public DateTime DateOfImplementation { get; set; }

        [Required]
        public VehicleStageStatus Status { get; set; }
    }
}
