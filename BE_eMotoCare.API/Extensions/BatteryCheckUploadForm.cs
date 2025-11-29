using System.ComponentModel.DataAnnotations;

namespace BE_eMotoCare.API.Extensions
{
    public class BatteryCheckUploadForm
    {
        public Guid EVCheckDetailId { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
