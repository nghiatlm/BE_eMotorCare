

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class MaintenanceStageResponse
    {
        public Guid Id { get; set; }
        public MaintenancePlanResponse? MaintenancePlan { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Mileage Mileage { get; set; }
        public DurationMonth DurationMonth { get; set; }
        public int? EstimatedTime { get; set; }
        public Status Status { get; set; }
        public ICollection<MaintenanceStageDetailResponse>? MaintenanceStageDetails { get; set; }
    }
}
