namespace eMotoCare.BO.DTO.Responses
{
    public class VehicleHistoryResponse
    {
        public VehicleResponse Vehicle { get; set; } = default!;
        public List<AppointmentResponse> MaintenanceHistory { get; set; } = new();
        public List<AppointmentResponse> RepairHistory { get; set; } = new();
        public List<AppointmentResponse> WarrantyHistory { get; set; } = new();
        public List<CampaignResponse> CampaignHistory { get; set; } = new();
    }
}
