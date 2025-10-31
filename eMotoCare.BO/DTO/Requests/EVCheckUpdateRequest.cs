

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class EVCheckUpdateRequest
    {
        [Required]
        public DateTime CheckDate { get; set; }
        public decimal? TotalAmout { get; set; }
        [Required]
        [EnumDataType(typeof(EVCheckStatus))]
        public EVCheckStatus Status { get; set; }
        public int Odometer { get; set; }
        [Required]
        public Guid AppointmentId { get; set; }
        [Required]
        public Guid TaskExecutorId { get; set; }
    }
}
