using System.ComponentModel.DataAnnotations;

namespace BE_eMotoCare.API.Extensions
{
    public class MaintenanceStageImport
    {
        [Required]
        public IFormFile File { get; set; } = default!;

        public string Format { get; set; } = "csv";
    }
}
