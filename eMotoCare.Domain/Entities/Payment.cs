using eMotoCare.Domain.Common;
using eMotoCare.Domain.Enums;

namespace eMotoCare.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public Guid PaymentId { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid PayerId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public virtual Appointment? Appointment { get; set; }
        public virtual Customer? Payer { get; set; }

    }
}