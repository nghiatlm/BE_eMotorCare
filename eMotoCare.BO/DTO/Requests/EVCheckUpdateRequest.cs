

using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class EVCheckUpdateRequest
    {
        public DateTime? CheckDate { get; set; }
        public decimal? TotalAmout { get; set; }
        [EnumDataType(typeof(EVCheckStatus))]
        public EVCheckStatus? Status { get; set; }
        public int? Odometer { get; set; }
        public Guid? AppointmentId { get; set; }
        public Guid? TaskExecutorId { get; set; }
    }
}
