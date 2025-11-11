

using eMotoCare.BO.Entities;
using eMotoCare.BO.Enums;


namespace eMotoCare.BO.DTO.Responses
{
    public class EVCheckResponse
    {
        public Guid Id { get; set; }
        public DateTime CheckDate { get; set; }
        public decimal? TotalAmout { get; set; }
        public EVCheckStatus Status { get; set; }
        public int Odometer { get; set; }
        public string VinNumber { get; set; }
        public AppointmentResponse? Appointment { get; set; }
        public StaffResponse? TaskExecutor { get; set; }
        public ICollection<EVCheckDetailResponse>? EVCheckDetails { get; set; }

    }
}
