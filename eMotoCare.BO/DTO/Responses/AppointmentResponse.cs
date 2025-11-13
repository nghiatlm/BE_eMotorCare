using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class AppointmentResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public Guid ServiceCenterId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? ApproveById { get; set; }
        public Guid? VehicleStageId { get; set; }
        public MaintenanceStageResponse? MaintenanceStage { get; set; }
        public DateTime AppointmentDate { get; set; }
        public SlotTime SlotTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public ServiceType Type { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? CheckinCode { get; set; }
        public ServiceCenterResponse? ServiceCenter { get; set; }
        public CustomerResponse? Customer { get; set; }
        public string? CheckinQRCode { get; set; }
        public Guid? EVCheckId { get; set; }
        public string? Note { get; set; }
    }
}
