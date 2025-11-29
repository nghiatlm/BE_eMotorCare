namespace eMotoCare.BO.DTO.Responses
{
    public class FirstVisitVehicleInfoResponse
    {
        public CustomerResponse Customer { get; set; } = default!;
        public VehicleResponse Vehicle { get; set; } = default!;
        public VehicleStageResponse? VehicleStage { get; set; }
    }
}
