using eMotoCare.BO.Enum;

namespace eMotoCare.BO.DTO.Responses
{
    public class ModelResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;

        public Guid MaintenancePlanId { get; set; }
        public MaintenancePlanResponse? MaintenancePlan { get; set; }

        public Status Status { get; set; }

        public List<VehicleResponse>? Vehicles { get; set; }
        //public List<ModelPartTypeResponse>? ModelPartTypes { get; set; }
    }
}
