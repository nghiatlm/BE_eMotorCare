

using eMotoCare.BO.Enum;
using System.ComponentModel.DataAnnotations;

namespace eMotoCare.BO.DTO.Requests
{
    public class MaintenanceStageDetailUpdateRequest
    {
        [Required]
        public Guid MaintenanceStageId { get; set; }
        [Required]
        public Guid PartId { get; set; }
        [Required]
        public ActionType[] ActionType { get; set; }
        public string? Description { get; set; }
        [Required]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }
    }
}
