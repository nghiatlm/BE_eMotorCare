namespace eMotoCare.BO.DTO.Responses
{
    public class FirstVisitVehicleInfoResponse
    {
        public CustomerResponse Customer { get; set; } = default!;
        public VehicleResponse Vehicle { get; set; } = default!;
        public List<VehicleStageShortResponse> VehicleStages { get; set; } = new();
        public List<VehiclePartItemResponse> VehiclePartItems { get; set; } = new();
    }
}
