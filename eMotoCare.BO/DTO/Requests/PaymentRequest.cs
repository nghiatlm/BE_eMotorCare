using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class PaymentRequest
    {
        [Required]
        public decimal? Amount { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentMethod))]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [EnumDataType(typeof(EnumCurrency))]
        public EnumCurrency Currency { get; set; } = EnumCurrency.VND;

        [Required]
        public Guid AppointmentId { get; set; }
    }
}
