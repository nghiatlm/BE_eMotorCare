

namespace eMotoCare.BO.DTO.Responses
{
    public class AppointmentDashboardMonthItem
    {
        public int Month { get; set; }
        public int Total { get; set; }
        public int CheckedIn { get; set; }
        public int Completed { get; set; }
        public int WaitingForPayment { get; set; }
        public int Maintenance { get; set; }
        public int Repair { get; set; }
        public int Warranty { get; set; }
        public int Campaign { get; set; }
        public int Recall { get; set; }
    }
}
