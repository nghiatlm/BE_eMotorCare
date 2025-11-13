namespace eMotoCare.BO.DTO.Responses
{
    public class MissingPartResponse
    {
        public Guid PartId { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public int NeededQty { get; set; }
        public int AvailableQty { get; set; }
        public int MissingQty { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid ServiceCenterId { get; set; }
        public string ServiceCenterName { get; set; } = "";
        public Guid? TaskExecutorId { get; set; }
        public DateTime? RequestedAt { get; set; }
        public Guid? CreatedById { get; set; }
        public string Status { get; set; } = "";
        public string? Note { get; set; }
        public List<MissingPartDetailResponse> Details { get; set; } = new();
    }
}
