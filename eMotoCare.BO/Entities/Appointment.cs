
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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



        public virtual ICollection<Payment>? Payments { get; set; }
    }
}