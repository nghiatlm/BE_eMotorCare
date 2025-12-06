using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class VehicleStageRequest
    {
        public Guid MaintenanceStageId { get; set; }
        public int ActualMaintenanceMileage { get; set; }
        public MaintenanceUnit ActualMaintenanceUnit { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public VehicleStageStatus Status { get; set; }
    }
}
