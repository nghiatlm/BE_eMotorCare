using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class AppointmentRequest
    {
        public Guid ServiceCenterId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? VehicleStageId { get; set; }
        public Guid? VehicleId { get; set; }
        public SlotTime SlotTime { get; set; }
        public Guid? CampaignId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }
        public AppointmentStatus Status { get; set; }
        public ServiceType Type { get; set; }
        public string? Note { get; set; }
        public Guid? RmaId { get; set; }
    }
}
