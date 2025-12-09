

namespace eMotoCare.BO.DTO.Responses
{
    public class VehicleStageShortResponse
    {
        public Guid Id { get; set; }            
        public Guid VehicleId { get; set; }
        public Guid MaintenanceStageId { get; set; }

        public string? MaintenanceStageName { get; set; }  
        public string Status { get; set; } = null!;     

        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public DateTime? ExpectedImplementationDate { get; set; }
    }
}
