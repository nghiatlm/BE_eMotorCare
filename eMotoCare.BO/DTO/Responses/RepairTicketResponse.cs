namespace eMotoCare.BO.DTO.Responses
{
    public class RepairTicketResponse
    {
        public Guid AppointmentId { get; set; }
        public string AppointmentCode { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public EVCheckResponse EVCheck { get; set; } = new();
        public double TotalMustPay { get; set; }
    }
}
