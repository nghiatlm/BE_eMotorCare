


using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eMotoCare.BO.DTO.Requests
{
    public class MaintenanceStageUpdateRequest
    {
        [Required]
        public Guid MaintenancePlanId { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        [Column("description")]
        public string? Description { get; set; }

        [Required]
        [EnumDataType(typeof(Mileage))]
        public Mileage Mileage { get; set; }

        [Required]
        [EnumDataType(typeof(DurationMonth))]
        public DurationMonth DurationMonth { get; set; }

        public int? EstimatedTime { get; set; }

        [Required]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
    }
}
