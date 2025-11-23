using System.ComponentModel.DataAnnotations;

namespace BE_eMotoCare.API.Extensions
{
    public class BatteryCheckUploadForm
    {
        [Required]
        public Guid EVCheckDetailId { get; set; }

        [Required]
        public IFormFile File { get; set; } = default!;
    }
}
