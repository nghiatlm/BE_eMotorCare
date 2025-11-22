namespace eMotoCare.BO.DTO.Responses
{
    public class MissingPartResponse
    {
        public Guid AppointmentId { get; set; }
        public string RequestCode { get; set; } = "";
        public DateTime RequestedAt { get; set; }
        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; } = "";
        public Guid ServiceCenterId { get; set; }
        public string ServiceCenterName { get; set; } = "";

        //public string Status { get; set; } = "";
        public string? Note { get; set; }
        public List<MissingPartDetailResponse> Details { get; set; } = new();
    }
}
