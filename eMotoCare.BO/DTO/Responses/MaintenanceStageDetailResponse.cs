

using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class MaintenanceStageDetailResponse
    {
        public Guid Id { get; set; }
        public Guid MaintenanceStageId { get; set; }
        public PartMaintenanceStageDetailResponse? Part { get; set; }
        public ActionType ActionType { get; set; }
        public string? Description { get; set; }
    }
}
