using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class VehicleStageResponse
    {
        public Guid Id { get; set; }
        public Guid MaintenanceStageId { get; set; }
        public MaintenanceStageResponse? MaintenanceStage { get; set; }
        public int ActualMaintenanceMileage { get; set; }
        public MaintenanceUnit ActualMaintenanceUnit { get; set; }
        public Guid VehicleId { get; set; }
        public VehicleResponse? Vehicle { get; set; }
        public DateTime DateOfImplementation { get; set; }
        public VehicleStageStatus Status { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public DateTime? ExpectedImplementationDate { get; set; }
        public DateTime? ActualImplementationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
