using eMotoCare.BO.Enums;

namespace eMotoCare.BO.DTO.Responses
{
    public class ServiceCenterSlotResponse
    {
        public Guid Id { get; set; }
        public Guid ServiceCenterId { get; set; }
        public DayOfWeeks DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public string? Note { get; set; }
    }
}
