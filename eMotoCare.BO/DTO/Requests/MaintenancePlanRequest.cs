

using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class MaintenancePlanRequest
    {

        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public MaintenanceUnit[] Unit { get; set; }
        [Required]
        public int TotalStages { get; set; }
        [Required]
        public DateTime EffectiveDate { get; set; }

    }
}
