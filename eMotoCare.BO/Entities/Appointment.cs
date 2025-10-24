using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.Entities
{
    [Table("appointment")]
    public class Appointment
    {
        [Key]
        [Column("appointment_id")]
        public Guid Id { get; set; }

        [Column("approve_by_id")]
        public Guid? ApproveById { get; set; }

        [ForeignKey(nameof(ApproveById))]
        public virtual Staff? ApproveBy { get; set; }

        [Required]
        [Column("service_center_id")]
        public Guid ServiceCenterId { get; set; }

        [ForeignKey(nameof(ServiceCenterId))]
        public virtual ServiceCenter? ServiceCenter { get; set; }

        [Column("customer_id")]
        public Guid CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }

        [InverseProperty(nameof(EVCheck.Appointment))]
        public virtual EVCheck? EVCheck { get; set; }

        [Column("vehicle_stage_id")]
        public Guid? VehicleStageId { get; set; }

        [ForeignKey(nameof(VehicleStageId))]
        public virtual VehicleStage? VehicleStage { get; set; }

        [Column("campaign_id")]
        public Guid? CampaignId { get; set; }

        [ForeignKey(nameof(CampaignId))]
        public virtual Campaign? Campaign { get; set; }

        [Required]
        [StringLength(50)]
        [Column("code")]
        public string Code { get; set; } = default!;

        [Required]
        [Column("appointment_date")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(50)]
        [Column("time_slot")]
        public string TimeSlot { get; set; } = default!;

        [Column("estimated_cost", TypeName = "decimal(18,2)")]
        public decimal? EstimatedCost { get; set; }

        [Column("actual_cost", TypeName = "decimal(18,2)")]
        public decimal? ActualCost { get; set; }

        [Required]
        [Column("status", TypeName = "varchar(200)")]
        [EnumDataType(typeof(AppointmentStatus))]
        public AppointmentStatus Status { get; set; }

        [Required]
        [Column("type", TypeName = "varchar(200)")]
        [EnumDataType(typeof(ServiceType))]
        public ServiceType Type { get; set; }

        [Column("checkin_qr_code", TypeName = "varchar(200)")]
        public string? CheckinQRCode { get; set; }

        [Column("service_center_slot_id")]
        public Guid? ServiceCenterSlotId { get; set; }

        [ForeignKey(nameof(ServiceCenterSlotId))]
        public virtual ServiceCenterSlot? ServiceCenterSlot { get; set; }

        public virtual ICollection<Payment>? Payments { get; set; }
    }
}
