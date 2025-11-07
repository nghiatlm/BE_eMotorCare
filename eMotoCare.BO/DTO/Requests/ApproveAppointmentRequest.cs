using System.ComponentModel.DataAnnotations;

namespace BE_eMotoCare.API.Controllers
{
    public class ApproveAppointmentRequest
    {
        [Required]
        public Guid StaffId { get; set; }

        [Required]
        public string CheckinQRCode { get; set; } = default!;
    }
}
