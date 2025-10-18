

using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class MaintenancePlanResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public MaintenanceUnit[] Unit { get; set; }
        public int TotalStages { get; set; }
        public DateTime EffectiveDate { get; set; }
        public Status Status { get; set; }
    }
}
