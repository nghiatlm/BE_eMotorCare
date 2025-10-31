﻿using System.ComponentModel.DataAnnotations;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Requests
{
    public class AppointmentRequest
    {
        [Required]
        public Guid ServiceCenterId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public Guid? VehicleStageId { get; set; }

        [Required]
        public Guid ServiceCenterSlotId { get; set; }

        [Required]
        [StringLength(50)]
        public string TimeSlot { get; set; } = default!;

        [Required]
        public DateTime AppointmentDate { get; set; }

        public decimal? EstimatedCost { get; set; }
        public decimal? ActualCost { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; }

        [Required]
        public ServiceType Type { get; set; }
    }
}
