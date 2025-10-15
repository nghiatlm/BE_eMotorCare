
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.Entities
{
    [Table("payment")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public Guid Id { get; set; }

        [Required]
        [Column("appointment_id")]
        public Guid AppointmentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment? Appointment { get; set; }

        [Required]
        [Column("customer_id")]
        public Guid CustomerID { get; set; }

        [ForeignKey(nameof(CustomerID))]
        public virtual Customer? Customer { get; set; }
    }
}