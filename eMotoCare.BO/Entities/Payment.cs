
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.Entities
{
    [Table("payment")]
    public class Payment
    {
        [Key]
        [Column("payment_id")]
        public Guid Id { get; set; }

        [Column("transaction_code", TypeName = "varchar(150)")]
        public string? TransactionCode { get; set; }

        [Column("status", TypeName = "varchar(100)")]
        [Required]
        [EnumDataType(typeof(StatusPayment))]
        public StatusPayment Status { get; set; }

        [Column("payment_method", TypeName = "varchar(150)")]
        [Required]
        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        [Column("appointment_id")]
        public Guid AppointmentId { get; set; }

        [Column("amount")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
        public double Amount { get; set; }

        [Column("currency", TypeName = "varchar(10)")]
        [Required]
        [EnumDataType(typeof(EnumCurrency))]
        public EnumCurrency Currency { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment? Appointment { get; set; }


    }
}