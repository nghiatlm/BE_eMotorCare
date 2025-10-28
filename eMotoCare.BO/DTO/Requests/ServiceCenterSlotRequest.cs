using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Requests
{
    public class ServiceCenterSlotRequest
    {
        public DayOfWeeks DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Note { get; set; }
    }
}
