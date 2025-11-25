using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class ModelRequest
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Manufacturer { get; set; } = string.Empty;

        [Required]
        public Guid MaintenancePlanId { get; set; }
    }
}
