

namespace eMotoCare.BO.DTO.Responses
{
    public class AppointmentDashboardResponse
    {
        public int Year { get; set; }
        public List<AppointmentDashboardMonthItem> Data { get; set; } = new();
    }
}
