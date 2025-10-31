

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class PaymentRequest
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [EnumDataType(typeof(EnumCurrency))]
        public EnumCurrency Currency { get; set; }

        [Required]
        public Guid AppointmentId { get; set; }

        [Required]
        public Guid CustomerID { get; set; }
    }
}
