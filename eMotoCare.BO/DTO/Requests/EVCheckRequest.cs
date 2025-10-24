

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class EVCheckRequest
    {
        [Required]
        public DateTime CheckDate { get; set; }
        [Required]
        public decimal? TotalAmout { get; set; }
        [Required]
        public int Odometer { get; set; }
        [Required]
        public Guid AppointmentId { get; set; }
        [Required]
        public Guid TaskExecutorId { get; set; }

    }
}
