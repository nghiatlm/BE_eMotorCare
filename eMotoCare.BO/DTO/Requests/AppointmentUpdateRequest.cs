using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class AppointmentUpdateRequest
    {
        public SlotTime? SlotTime { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }
        public string? Note { get; set; }
        public Guid? ApproveById { get; set; }
        public string? Code { get; set; }
        public string? CheckinQRCode { get; set; }
    }
}
