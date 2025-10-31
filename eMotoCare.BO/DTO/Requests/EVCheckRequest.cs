

using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class EVCheckRequest
    {

        [Required]
        public Guid AppointmentId { get; set; }
        [Required]
        public Guid TaskExecutorId { get; set; }

    }
}
