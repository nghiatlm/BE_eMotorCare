using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class AppointmentRequest
    {
        [Required]
        public Guid ServiceCenterId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        public Guid? VehicleStageId { get; set; }
        public Guid? VehicleId { get; set; }

        [Required]
        public SlotTime SlotTime { get; set; }
        public Guid? CampaignId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        public decimal? EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        [Required]
        public ServiceType Type { get; set; }
        public string? Note { get; set; }
    }
}
