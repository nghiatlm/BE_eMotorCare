

using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class MaintenanceStageDetailRequest
    {
        [Required]
        public Guid MaintenanceStageId { get; set; }
        [Required]
        public Guid PartId { get; set; }
        [Required]
        [EnumDataType(typeof(ActionType))]
        public ActionType ActionType { get; set; }
        public string? Description { get; set; }
    }
}
